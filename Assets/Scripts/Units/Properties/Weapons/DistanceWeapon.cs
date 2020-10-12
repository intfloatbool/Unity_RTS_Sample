using System.Collections.Generic;
using System.Linq;
using Game.Weapons;
using UnityEngine;

namespace Units.Properties.Weapons
{
    public class DistanceWeapon : Weapon
    {
        public enum DistanceLaunchType: byte
        {
            NONE = 0,
            CONSTANT = 1,
            SIEGE = 2
        }
        
        [SerializeField] private Missile _ammoPrefab;
        [SerializeField] private Transform _launchPos;
        [SerializeField] private DistanceLaunchType _launchType = DistanceLaunchType.CONSTANT;
        
        private List<Missile> _ammoPool = new List<Missile>(10);
        private Transform _poolHolder;


        protected override void Awake()
        {
            base.Awake();
            _poolHolder = new GameObject($"{transform.root.name}_{transform.root.GetInstanceID().ToString()}_POOL")
                .transform;

            if (_launchPos == null)
            {
                _launchPos = transform;
            }
        }
        
        protected override void UseWeapon()
        {
            _isReady = false;
            var ammo = GetAmmoFromPool();
            ammo.transform.position = _launchPos.position;
            if (_launchType == DistanceLaunchType.CONSTANT)
            {
                ammo.Init(this, Target);
            }
            else if(_launchType == DistanceLaunchType.SIEGE)
            {
                ammo.Init(this, Target.transform.position);
            }
        }

        public void DamageTarget(GameUnit target)
        {
            if (target != null)
            {
                DamageToCustomTargetNormalized(target);
            }
        }

        private Missile GetAmmoFromPool()
        {
            Missile freeAmmo = _ammoPool.FirstOrDefault(amm => !amm.gameObject.activeInHierarchy);
            if (freeAmmo == null)
            {
                freeAmmo = Instantiate(_ammoPrefab, _poolHolder);
                _ammoPool.Add(freeAmmo);
            }
            else
            {
                freeAmmo.gameObject.SetActive(true);
            }

            return freeAmmo;
        }
    }

}
