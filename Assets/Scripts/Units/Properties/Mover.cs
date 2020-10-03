using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace Units.Properties
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _moveSpeed;

        public float MoveSpeed
        {
            get => _moveSpeed;
            set
            {
                _moveSpeed = value;
                if (_navMeshAgent != null)
                {
                    _navMeshAgent.speed = _moveSpeed;
                }
            }
        }
        private void Awake()
        {
            Assert.IsNotNull(_navMeshAgent, "_navMeshAgent != null");
            if (_navMeshAgent != null)
            {
                _navMeshAgent.speed = _moveSpeed;
            }
        }

        public void MoveTo(Vector3 point)
        {
            if (_navMeshAgent == null)
                return;
            _navMeshAgent.SetDestination(point);
        }

        public void MoveToByOffset(Vector3 offset)
        {
            if (_navMeshAgent == null)
                return;
            
            _navMeshAgent.Move(offset);
        }
    }
}