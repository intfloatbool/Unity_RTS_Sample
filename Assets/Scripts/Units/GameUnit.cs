using System;
using JetBrains.Annotations;
using Units.Enums;
using Units.Properties;
using UnityEngine;

namespace Units
{
    public class GameUnit : MonoBehaviour
    {
        [SerializeField] private UnitState _currentState = UnitState.STANDING;
        public UnitState CurrentState => _currentState;

        [Space]
        [SerializeField] private int _maxHealth = 100;
        
        public int MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }

        [SerializeField] private int _maxMana = 0;

        public int MaxMana
        {
            get => _maxMana;
            set => _maxMana = value;
        }

        [SerializeField] private int _armor = 0;

        public int Armor
        {
            get => _armor;
            set => _armor = value;
        }
        
        [Space]
        [SerializeField] private float _hpRegeneration = 0.5f;

        public float HpRegeneration
        {
            get => _hpRegeneration;
            set => _hpRegeneration = value;
        }

        [SerializeField] private float _manaRegeneration = 0.5f;

        public float ManaRegeneration
        {
            get => _manaRegeneration;
            set => _manaRegeneration = value;
        }

        [Space]
        [SerializeField] private PlayerOwner _owner;
        
        public PlayerOwner Owner
        {
            get => _owner;
            set => _owner = value;
        }

        [Space] 
        [SerializeField] 
        [CanBeNull] private Weapon _weapon;

        [CanBeNull]
        public Weapon Weapon
        {
            get => _weapon;
            set => _weapon = value;
        }
        
        [SerializeField] 
        [CanBeNull] private Mover _mover;

        [CanBeNull]
        public Mover Mover
        {
            get => _mover;
            set => _mover = value;
        }
        
        [SerializeField] 
        [CanBeNull] private SpellCaster _spellCaster;

        [CanBeNull]
        public SpellCaster SpellCaster
        {
            get => _spellCaster;
            set => _spellCaster = value;
        }


        #region UnitEvents

        public event Action<UnitState> OnStateChanged;

        #endregion

        private void SetState(UnitState unitState)
        {
#if UNITY_EDITOR
            Debug.Log($"State changed: lastState: {_currentState},current: {unitState}");
#endif
            _currentState = unitState;
            
            OnStateChanged?.Invoke(_currentState);
        }
        public void DoAction(UnitActionType actionType)
        {
            switch (_currentState)
            {
                case UnitState.STANDING:
                {
                    if (actionType == UnitActionType.MOVE_START)
                    {
                        SetState(UnitState.MOVING);
                    }
                    else if (actionType == UnitActionType.ATTACK_START)
                    {
                        SetState(UnitState.ATTACKING);
                    }
                    else if (actionType == UnitActionType.SPELL_START)
                    {
                        SetState(UnitState.SPELLING);
                    }
                    else if (actionType == UnitActionType.DIE)
                    {
                        SetState(UnitState.DYING);
                    }
                    break;
                }
                case UnitState.MOVING:
                {
                    if (actionType == UnitActionType.MOVE_STOP)
                    {
                        SetState(UnitState.STANDING);
                    }

                    if (actionType == UnitActionType.DIE)
                    {
                        SetState(UnitState.DYING);
                    }
                    break;
                }
                case UnitState.SPELLING:
                {
                    if (actionType == UnitActionType.SPELL_STOP)
                    {
                        SetState(UnitState.STANDING);
                    }
                    if (actionType == UnitActionType.DIE)
                    {
                        SetState(UnitState.DYING);
                    }
                    break;
                }
                case UnitState.ATTACKING:
                {
                    if (actionType == UnitActionType.ATTACK_STOP)
                    {
                        SetState(UnitState.STANDING);
                    }
                    if (actionType == UnitActionType.DIE)
                    {
                        SetState(UnitState.DYING);
                    }
                    break;
                }
            }
        }
        
    }
}