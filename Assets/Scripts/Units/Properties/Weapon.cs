using System;
using Game.Services.SingletonServices;
using Game.Static;
using JetBrains.Annotations;
using Units.Properties.Enums;
using Units.Properties.Weapons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Units.Properties
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected AttackType _attackType;
        public AttackType AttackType => _attackType;
        
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

        [SerializeField] protected float _attackSpeed = 2f;

        public float AttackSpeed
        {
            get => _attackSpeed;
            set => _attackSpeed = value;
        }

        [CanBeNull]
        [SerializeField] protected WeaponDoneControllerBase _weaponDoneController;
        
        [Space]
        [Header("Runtime")]
        [SerializeField] protected bool _isReady = true;
        public bool IsReady => _isReady;

        public GameUnit Target { get; set; }
        public GameUnit Owner { get; set; }
        
        public event Action OnWeaponUsed;
        
        
        protected float _attackTimer = 0f;

        protected AttackHandler _attackHandler;

        private void Awake()
        {
            if (GameHelper.Services != null)
            {
                _attackHandler = GameHelper.Services.GetService<AttackHandler>();
                if (_attackHandler == null)
                {
                    Debug.LogError("Attack handler is missing!!");
                }
            }
            else
            {
                Debug.LogError("Services is missing!");
            }
        }

        protected void DamageToTargetNormalized()
        {
            if (Target == null)
                return;
            var randomDamage = RandomDamage;
            if (_attackHandler != null)
            {
                randomDamage = _attackHandler.NormalizeDamage(randomDamage, this, Target);
            }
            Target.MakeDamage(randomDamage, Owner);
        }


        protected virtual void Update()
        {
            if(_weaponDoneController == null)
                HandleAttackTimer();
        }

        public virtual void Attack(GameUnit sender)
        {
            
            if (_weaponDoneController != null && _weaponDoneController.IsWaitForDone)
            {
                return;
            }

            if (_weaponDoneController != null)
            {
                _weaponDoneController.WaitForAttackDone(this, WeaponUsed);
            }
            else
            {
                if(!_isReady)
                    return;
                
                WeaponUsed();
            }
            
            UseWeapon();
        }

        protected abstract void UseWeapon();

        protected virtual void WeaponUsed()
        {
            if (_weaponDoneController != null)
            {
                _isReady = true;
            }
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
                if (_attackTimer >= _attackSpeed)
                {
                    _isReady = true;
                }
                _attackTimer += Time.deltaTime;
            }
        }
    }
}