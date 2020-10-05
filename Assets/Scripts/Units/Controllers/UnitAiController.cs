using System;
using Units.Controllers.Enums;
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


        private float _roamTimer;


        private Vector3 _basicUnitPos;
        
        private NavMeshAgent _unitNavMesh;
        private void Awake()
        {
            if (_gameUnit != null)
            {
                _unitNavMesh = _gameUnit.GetComponent<NavMeshAgent>();
            }
            
            Assert.IsNotNull(_unitNavMesh, "_unitNavMesh != null");
        }

        private void Start()
        {
            SaveBasicValues();
        }

        private void SaveBasicValues()
        {
            _basicUnitPos = transform.position;
        }

        protected override void ControllUnitLoop()
        {
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
                    break;
                case AiBehaviorType.STAND:
                    break;
                case AiBehaviorType.STAND_AND_ATTACK_ENEMIES:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleRoamingLoop()
        {
            RoamTimerLoop();
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
            _unitNavMesh.destination = finalPosition;
        }
    } 
}