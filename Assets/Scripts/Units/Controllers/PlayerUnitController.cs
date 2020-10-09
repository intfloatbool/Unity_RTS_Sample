using Game.Static;
using GameInput;
using Units.Enums;
using Units.Properties;
using Units.Properties.Weapons;
using UnityEngine;
using UnityEngine.Assertions;

namespace Units.Controllers
{
    public class PlayerUnitController : UnitControllerBase
    {
        [SerializeField] private GameUnit _defaultUnit;
        private GameUnit _lastUsedUnit;

        [Space]
        [SerializeField] private InputProviderBase _inputProvider;
        [SerializeField] private float _rotationSpeed = 30f;
        [SerializeField] private float _meleeAttackSphereCastRadius = 0.3f;
        
        [Space]
        [Header("Runtime")]
        [SerializeField] private Vector3 _moveDir;

        private bool _isMoving;

        private float _moveDirStrength = 1f;
        
        private Animator _animator;

        private void Awake()
        {
            Assert.IsNotNull(_inputProvider, "_inputProvider != null");
            if (_inputProvider != null)
            {
                _inputProvider.OnButtonPressed += OnButtonPressed;
            }

            if (_defaultUnit != null)
            {
                SetUnit(_defaultUnit);
            }

        }

        private void OnDestroy()
        {
            if (_inputProvider != null)
            {
                _inputProvider.OnButtonPressed -= OnButtonPressed;
            }
        }

        public override void SetUnit(GameUnit gameUnit)
        {
            base.SetUnit(gameUnit);
            if (_gameUnit != null)
            {
                if (_lastUsedUnit != null)
                {
                    var weapon = _lastUsedUnit.Weapon;

                    if (weapon != null)
                    {
                        weapon.OnWeaponUsed -= OnWeaponAttackDone;
                    }
                }
                
                var currentWeapon = _gameUnit.Weapon;

                if (currentWeapon != null)
                {
                    currentWeapon.OnWeaponUsed += OnWeaponAttackDone;
                }

                _animator = _gameUnit.GetComponentInChildren<Animator>();

                _lastUsedUnit = _gameUnit;
            }
        }

        private void OnWeaponAttackDone()
        {
            _gameUnit.DoAction(UnitActionType.ATTACK_STOP);
        }

        protected override void ControllUnitLoop()
        {
            if (_inputProvider == null)
                return;
            
            PlayerInputControlLoop();
        }

        private void PlayerInputControlLoop()
        {
            
            
            bool isMoved = false;
            
            // Disable moving when attacking
            if (_gameUnit.CurrentState != UnitState.ATTACKING)
            {
                isMoved = ControlMovementLoop();
            }
            
            ControlRotationLoop();

            bool isInputDown = isMoved;
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
            _moveDirStrength = y;

            if (_animator != null)
            {
                _animator.SetFloat(GameHelper.Animations.MOVE_DIR_PARAM, _moveDirStrength);
            }
            
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
            if(_gameUnit == null)
                return;
            if (_gameUnit.IsDead)
                return;
            
            switch (inputButton)
            {
                case InputButton.ATTACK:
                {
                    if (_gameUnit.CurrentState != UnitState.MOVING
                        && _gameUnit.CurrentState != UnitState.SPELLING)
                    {
                        OnAttack(); 
                    }
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
            var weapon = _gameUnit.Weapon;
            if (weapon != null && weapon.IsReady)
            {
                _gameUnit.DoAction(UnitActionType.ATTACK_START);
                HandleAttackByWeaponType(weapon);
            }
        }

        private void HandleAttackByWeaponType(Weapon weapon)
        {
            if (weapon != null)
            {
                if (weapon is MeleeWeapon)
                {
                    GameUnit target = null;
                    RaycastHit hit;
                    var ray = new Ray(weapon.transform.position, _gameUnit.transform.forward);
                    Debug.DrawRay(ray.origin, ray.direction, Color.red, 2f);
                    
                    if (Physics.SphereCast(ray, _meleeAttackSphereCastRadius, out hit, weapon.AttackDistance))
                    {
                        if (hit.collider.CompareTag(GameHelper.GameTags.GAME_UNIT))
                        {
                            var unit = hit.collider.GetComponent<GameUnit>();
                            if (unit != null && unit.Owner != _gameUnit.Owner)
                            { 
                                target = unit;
                            }
                        }

                        weapon.Target = target;
                    }
                }
                
                weapon.Attack(_gameUnit);
            }
        }

        private void OnSpell()
        {
            // TODO: Complete spell stop
            _gameUnit.DoAction(UnitActionType.SPELL_START);
        }
    }
}