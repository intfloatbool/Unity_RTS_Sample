using System;
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

        [Space] 
        [Header("Runtime")]
        [SerializeField] private GameUnit _targetUnit;
        [SerializeField] private Vector3 _pointTarget;
        
        private float _roamTimer;
        
        private Vector3 _basicUnitPos;
        
        private NavMeshAgent _unitNavMesh;


        private bool _isMoving;

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
            
            //test
            Debug.Log($"{_gameUnit.name} attack done!");
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
            
            HandleAiBehaviourLoop();
        }

        private void HandleAiBehaviourLoop()
        {
            switch (_aiBehaviorType)
            {
                case AiBehaviorType.NONE:
                    break;
                case AiBehaviorType.MOVE_TO_TARGET:
                    break;
                case AiBehaviorType.MOVE_TO_TARGET_AND_ATTACK_ENEMIES:
                    break;
                case AiBehaviorType.ROAMING:
                    HandleRoamingLoop();
                    break;
                case AiBehaviorType.ROAMING_AND_ATTACK_ENEMIES:
                    HandleRoamingAndAttackLoop();
                    break;
                case AiBehaviorType.STAND:
                    break;
                case AiBehaviorType.STAND_AND_ATTACK_ENEMIES:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleRoamingAndAttackLoop()
        {
            HandleAttackLoop();
            if(_gameUnit.CurrentState != UnitState.ATTACKING)
                HandleRoamingLoop();
        }

        private void HandleAttackLoop()
        {
            var weapon = _gameUnit?.Weapon;
            if (weapon != null)
            {
                bool isReachTarget = IsReachTargetUnit(weapon.AttackDistance);
                if (isReachTarget)
                {
                    // Look at enemy is necessary
                    _unitNavMesh.transform.LookAt(_targetUnit.transform.position);
        
                    if (weapon.IsReady)
                    {
                        _gameUnit.DoAction(UnitActionType.ATTACK_START);
                        weapon.Attack();
                    }
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
            
            var roamingRadiusColor = Color.cyan;

            Gizmos.color = roamingRadiusColor;
            var center = _isStaticRoaming ? _basicUnitPos : _gameUnit.transform.position;
            Gizmos.DrawWireSphere(center, _roamRadius);
        }
    } 
}