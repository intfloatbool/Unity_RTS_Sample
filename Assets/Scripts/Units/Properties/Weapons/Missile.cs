using Units;
using Units.Properties;
using Units.Properties.Weapons;
using UnityEngine;

namespace Game.Weapons
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] private float _speed = 6f;
        [SerializeField] private float _minDistanceToAttack = 0.06f;
        public bool IsLaunched { get; private set; }
        public GameUnit Target { get; private set; }
        public DistanceWeapon Weapon { get; private set; }
        
        public Vector3 TargetPos { get; private set; }
        public void Init(DistanceWeapon weapon, GameUnit target)
        {
            Weapon = weapon;
            Target = target;
            IsLaunched = true;
        }
        
        public void Init(DistanceWeapon weapon, Vector3 targetPos)
        {
            Weapon = weapon;
            Target = null;
            TargetPos = targetPos;
            IsLaunched = true;
        }

        private void Update()
        {
            if (!IsLaunched)
                return;

            if (Target != null)
            {
                transform.LookAt(Target.transform);
                TargetPos = Target.transform.position;
            }

            transform.position = Vector3.MoveTowards(transform.position, TargetPos,
                _speed * Time.deltaTime);
            
            Vector3 offset = TargetPos - transform.position;
            float sqrLen = offset.sqrMagnitude;
            
            if (sqrLen < _minDistanceToAttack * _minDistanceToAttack)
            {
                Bang();
                IsLaunched = false;
            }
        }

        protected virtual void Bang()
        {
            Weapon?.DamageTarget(Target);
            gameObject.SetActive(false);
        }
    }
}

