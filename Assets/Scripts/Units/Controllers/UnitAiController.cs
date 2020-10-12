using System;
using Game.Static;
using Units.Controllers.Enums;
using Units.Enums;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Units.Controllers
{
    public class UnitAiController : UnitControllerBase
    {
        [SerializeField] private AiBehaviorType _aiBehaviorType;

        public AiBehaviorType AiBehaviorType
        {
            get => _aiBehaviorType;
            set => _aiBehaviorType = value;
        }
        
        [Space]
        [Header("Roaming")]
        [SerializeField] private bool _isStaticRoaming = false;
        [SerializeField] private float _roamRadius = 3f;
        [SerializeField] private float _roamTime = 3f;
        
        [Space]
        [Header("Attacking")]
        [SerializeField] private bool _isAutoDetectTarget;
        [SerializeField] private float _aggressiveRadius = 2.8f;
        
        
        [Space] 
        [Header("Runtime")]
        [SerializeField] private GameUnit _targetUnit;
        [SerializeField] private Vector3 _pointTarget;
        
        private float _roamTimer;
        
        private Vector3 _basicUnitPos;
        
        private NavMeshAgent _unitNavMesh;

        private GameObject _lastKilledTarget;
        
        private Collider[] _detectUnits = new Collider[10];
        
        
        private bool _isMoving;

        private void OnValidate()
        {
            if (_gameUnit == null)
                return;
            _basicUnitPos = _gameUnit.transform.position;
        }

        private void Awake()
        {
            if (_gameUnit != null)
            {
                _unitNavMesh = _gameUnit.GetComponent<NavMeshAgent>();
            }
            
            Assert.IsNotNull(_unitNavMesh, "_unitNavMesh != null");

            if (_gameUnit != null && _gameUnit.Weapon != null)
            {
                _gameUnit.Weapon.OnWeaponUsed += OnWeaponAttackDone;
            }
        }

        private void OnDestroy()
        {
            if (_gameUnit != null && _gameUnit.Weapon != null)
            {
                _gameUnit.Weapon.OnWeaponUsed -= OnWeaponAttackDone;
            }
        }

        private void OnWeaponAttackDone()
        {
            _gameUnit.DoAction(UnitActionType.ATTACK_STOP);
        }

        private void Start()
        {
            SaveBasicValues();
        }

        private void SaveBasicValues()
        {
            _basicUnitPos = transform.position;
        }

        public void SetTarget(GameUnit target)
        {
            _targetUnit = target;
        }

        public void SetTarget(Vector3 point)
        {
            _pointTarget = point;
        }

        protected override void ControllUnitLoop()
        {
            // leave this method empty for AI controller
        }

        private void LateUpdate()
        {
            // CRITICAL NECCESSARY USE THIS IN LateUpdate() Do not move it in Update in ControllUnitLoop() !
            if (_unitNavMesh == null)
                return;

            if (_gameUnit == null || _gameUnit.IsDead)
                return;

            if (_isAutoDetectTarget)
                AutoDetectTargetLoop();
            
            if (_targetUnit != null)
            {
                FollowToTargetAndAttackLoop();
            }
            else
            {
                HandleAiBehaviourLoop();  
            }
            
            
        }

        private void AutoDetectTargetLoop()
        {
            var weapon = _gameUnit.Weapon;
            
            if (_targetUnit != null)
            {
                if (_targetUnit.IsDead)
                {
                    _lastKilledTarget = _targetUnit.gameObject;
                    _targetUnit = null;
                }

                if (weapon != null)
                {
                    weapon.Target = _targetUnit;
                }
                
                return;
            }
            
            //clear
            for (int i = 0; i < _detectUnits.Length; i++)
            {
                _detectUnits[i] = null;
            }
            
            int detected = Physics.OverlapSphereNonAlloc(_gameUnit.transform.position, _aggressiveRadius, _detectUnits);
            if (detected > 0)
            {
                for (int i = 0; i < _detectUnits.Length; i++)
                {
                    var detectedUnit = _detectUnits[i];
                    if(detectedUnit == null)
                        continue;

                    if (!detectedUnit.transform.CompareTag(GameHelper.GameTags.GAME_UNIT))
                        continue;
                    
                    if(detectedUnit.gameObject.Equals(_lastKilledTarget))
                        continue;
                    
                    var gameUnit = detectedUnit.GetComponent<GameUnit>();
                    if (gameUnit != null)
                    {
                        if(gameUnit == _gameUnit)
                            continue;

                        if (gameUnit.IsDead)
                        {
                            continue;
                        }

                        if (gameUnit.Owner != _gameUnit.Owner)
                        {
                            _targetUnit = gameUnit;
                            _lastKilledTarget = null;
                        }
                    }
                }
            }
        }

        private void HandleAiBehaviourLoop()
        {
            switch (_aiBehaviorType)
            {
                case AiBehaviorType.NONE:
                    break;
                case AiBehaviorType.MOVE_TO_TARGET:
                    GoToTargetPointLoop();
                    break;
                case AiBehaviorType.ROAMING:
                    HandleRoamingLoop();
                    break;
                case AiBehaviorType.STAND:
                    StandLoop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StandLoop()
        {
            _gameUnit.DoAction(UnitActionType.MOVE_STOP);
        }

        private void GoToTargetPointLoop()
        {
            
            bool isReachPosition = IsReachPosition();
            if (isReachPosition)
            {
                _gameUnit.DoAction(UnitActionType.MOVE_STOP);
            }
            else
            {
                MoveToTarget(_pointTarget);
            }
            
        }

        private void FollowToTargetAndAttackLoop()
        {
            var weapon = _gameUnit?.Weapon;
            if (weapon != null)
            {
                bool isReachTarget = IsReachTargetUnit(weapon.AttackDistance);
                if (isReachTarget)
                {
                    _gameUnit.DoAction(UnitActionType.MOVE_STOP);
                    AttackTargetLoop();
                    _unitNavMesh.isStopped = true;
                }
                else
                {
                    _unitNavMesh.SetDestination(_targetUnit.transform.position);
                    _gameUnit.DoAction(UnitActionType.MOVE_START);   
                    _unitNavMesh.isStopped = false;
                }
                
            }
           
        }

        private void AttackTargetLoop()
        {
            // Look at enemy is necessary
            var weapon = _gameUnit?.Weapon;
            if (weapon != null)
            {
                _unitNavMesh.transform.LookAt(_targetUnit.transform.position);
                if (weapon.IsReady)
                {
                    _gameUnit.DoAction(UnitActionType.ATTACK_START);
                    weapon.Attack(_gameUnit);
                }
            }
        }
        

        private bool IsReachTargetUnit(float distanceToContact)
        {
            if (_targetUnit != null)
            {
                Vector3 offset = _targetUnit.transform.position -
                                 _gameUnit.transform.position;
                float sqrLen = offset.sqrMagnitude;
                float sqrDistance = distanceToContact * distanceToContact;

                if (sqrLen < sqrDistance)
                {
                    return true;
                }
            }

            return false;
        }

        private void HandleRoamingLoop()
        {
            RoamTimerLoop();
            bool isReachPosition = IsReachPosition();
            if (isReachPosition)
            {
                _gameUnit.DoAction(UnitActionType.MOVE_STOP);
            }
        }

        private void RoamTimerLoop()
        {
            if (_roamTimer >= _roamTime)
            {
                Roaming();
                _roamTimer = 0f;
            }
            _roamTimer += Time.deltaTime;
        }
        
        
        private void Roaming()
        {
            Vector3 randomDirection = Random.insideUnitSphere * _roamRadius;
            if (_isStaticRoaming)
            {
                randomDirection += _basicUnitPos;
            }
            else
            {
                randomDirection += _unitNavMesh.transform.position; 
            }
            
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, _roamRadius, 1);
            Vector3 finalPosition = hit.position;
            
            MoveToTarget(finalPosition);
        }

        private void MoveToTarget(Vector3 targetPoint)
        {
            _pointTarget = targetPoint;
            _unitNavMesh.destination = _pointTarget;
            
            _gameUnit.DoAction(UnitActionType.MOVE_START);
            _unitNavMesh.isStopped = false;
        }

        private bool IsReachPosition()
        {
            Vector3 offset = _pointTarget - _gameUnit.transform.position;
            float sqrLen = offset.sqrMagnitude;

            // square the distance we compare with
            float stoppingDistance = _unitNavMesh.stoppingDistance;
            if (sqrLen < stoppingDistance * stoppingDistance)
            {
                return true;
            }

            return false;
        }
        
        //DEBUG
        private void OnDrawGizmos()
        {
            if (_gameUnit == null)
                return;

            if (_isAutoDetectTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_gameUnit.transform.position, _aggressiveRadius);
            }

            var wayColor = Color.blue;
            wayColor.a = 0.3f;
            Gizmos.color = wayColor;
            Gizmos.DrawCube(_pointTarget, Vector3.one * 0.5f);
        }
    } 
}