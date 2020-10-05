using System;
using JetBrains.Annotations;
using Units.Properties.Weapons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Units.Properties
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected float _attackDistance = 1f;
        public float AttackDistance
        {
            get => _attackDistance;
            set => _attackDistance = value;
        }

        [SerializeField] protected int _minDamage = 5;
        public int MINDamage
        {
            get => _minDamage;
            set => _minDamage = value;
        }

        [SerializeField] protected int _maxDamage = 10;

        public int MAXDamage
        {
            get => _maxDamage;
            set => _maxDamage = value;
        }

        public int RandomDamage => Random.Range(_minDamage, _maxDamage);

        [SerializeField] protected float _attackDelay = 2f;

        public float AttackDelay
        {
            get => _attackDelay;
            set => _attackDelay = value;
        }

        [CanBeNull]
        [SerializeField] protected WeaponDoneControllerBase _weaponDoneController;
        
        [Space]
        [Header("Runtime")]
        [SerializeField] protected bool _isReady = true;
        public bool IsReady => _isReady;


        public event Action OnWeaponUsed;
        
        
        protected float _attackTimer = 0f;
        

        protected virtual void Update()
        {
            HandleAttackTimer();
        }

        public virtual void Attack()
        {
            if(!_isReady)
                return;
            
            UseWeapon();

            if (_weaponDoneController != null)
            {
                _weaponDoneController.WaitForAttackDone(this, WeaponUsed);
            }
            else
            {
                WeaponUsed();
            }
        }

        protected abstract void UseWeapon();

        protected virtual void WeaponUsed()
        {
            OnWeaponUsed?.Invoke();
        }

        private void HandleAttackTimer()
        {
            if (_isReady)
            {
                _attackTimer = 0f;
            }
            else
            {
                if (_attackTimer >= _attackDelay)
                {
                    _isReady = true;
                }
                _attackTimer += Time.deltaTime;
            }
        }
    }
}