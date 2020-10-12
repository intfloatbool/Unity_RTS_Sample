using System;
using Game.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace Units.Properties.Weapons
{
    public class AnimationWeaponDoneController : WeaponDoneControllerBase
    {
        [SerializeField] private Animator _animator;
        
        private Action _currentCallback;

        private float _timeToWait;
        private float _timer;

        private void Awake()
        {
            Assert.IsNotNull(_animator, "_animator != null");
        }

        public override void WaitForAttackDone(Weapon weapon, Action callBack)
        {
            _currentCallback = callBack;

            if (_animator != null)
            {
                _animator.SetFloat(GameHelper.Animations.ATTACK_SPEED_PARAM, weapon.AttackSpeed);
            }
        }

        public void StartWaiting(float length)
        {
            _timeToWait = length;
            _isWaitForDone = true;
        }

        private void Update()
        {
            if (_isWaitForDone)
            {
                if (_timer >= _timeToWait)
                {
                    _currentCallback?.Invoke();
                    _timer = 0f;
                    _isWaitForDone = false;
                }
                _timer += Time.deltaTime;
            }
            else
            {
                _timer = 0f;
            }
        }
    }
}