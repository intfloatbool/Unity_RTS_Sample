using JetBrains.Annotations;
using Units.Enums;
using Units.Properties;
using UnityEngine;

namespace Units
{
    public class GameUnit : MonoBehaviour
    {
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
    }
}