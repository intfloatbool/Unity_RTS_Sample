using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace Units.Properties
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            Assert.IsNotNull(_navMeshAgent, "_navMeshAgent != null");
        }

        public void MoveTo(Vector3 point)
        {
            if (_navMeshAgent == null)
                return;
            _navMeshAgent.SetDestination(point);
        }
    }
}