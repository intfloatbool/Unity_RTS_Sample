using System;
using GameInput;
using Units.Enums;
using UnityEngine;
using UnityEngine.Assertions;

namespace Units.Controllers
{
    public class PlayerUnitController : UnitControllerBase
    {
        [SerializeField] private InputProviderBase _inputProvider;
        [SerializeField] private float _rotationSpeed = 30f;
        
        [Space]
        [Header("Runtime")]
        [SerializeField] private Vector3 _moveDir;

        private bool _isMoving;
        
        protected override void Awake()
        {
            base.Awake();
            Assert.IsNotNull(_inputProvider, "_inputProvider != null");
            Assert.IsNotNull(_gameUnit.Mover, "_gameUnit.Mover != null");

            if (_inputProvider != null)
            {
                _inputProvider.OnButtonPressed += OnButtonPressed;
            }
        }

        private void OnDestroy()
        {
            if (_inputProvider != null)
            {
                _inputProvider.OnButtonPressed -= OnButtonPressed;
            }
        }

        protected override void ControllUnitLoop()
        {
            if (_inputProvider == null)
                return;
            
            PlayerInputControlLoop();
        }

        private void PlayerInputControlLoop()
        {
            bool isMoved = ControlMovementLoop();
            bool isRotated = ControlRotationLoop();

            bool isInputDown = isMoved || isRotated;
            if (isInputDown)
            {
                _gameUnit.DoAction(UnitActionType.MOVE_START);
            }
            else
            {
                _gameUnit.DoAction(UnitActionType.MOVE_STOP);
            }
        }

        private bool ControlMovementLoop()
        {
            var mover = _gameUnit.Mover;
            if (mover == null)
            {
                return false;
            }

            float y = _inputProvider.GetY();
            
            var offsetVector = _gameUnit.transform.forward * y;
            offsetVector = offsetVector * mover.MoveSpeed * Time.deltaTime;
            _moveDir = offsetVector;
            mover.MoveToByOffset(offsetVector);
            
            return !Mathf.Approximately(y, 0f);
        }

        private bool ControlRotationLoop()
        {
            float x = _inputProvider.GetX();
            _gameUnit.transform.Rotate(Vector3.up * x * _rotationSpeed * Time.deltaTime );

            return !Mathf.Approximately(x, 0f);
        }

        private void OnButtonPressed(InputButton inputButton)
        {
            switch (inputButton)
            {
                case InputButton.ATTACK:
                {
                    OnAttack();
                    break;
                }
                // TODO: Unify spells
                case InputButton.SKILL_1:
                {
                    OnSpell();
                    break;
                }
                case InputButton.SKILL_2:
                {
                    OnSpell();
                    break;
                }
                case InputButton.SKILL_3:
                {
                    OnSpell();
                    break;
                }
                case InputButton.SKILL_4:
                {
                    OnSpell();
                    break;
                }
            }
        }
        private void OnAttack()
        {
            // TODO: Complete attack stop
            _gameUnit.DoAction(UnitActionType.ATTACK_START);
        }

        private void OnSpell()
        {
            // TODO: Complete spell stop
            _gameUnit.DoAction(UnitActionType.SPELL_START);
        }
    }
}