using Units.Properties.Enums;
using UnityEngine;

namespace Game.Settings.Units
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Settings/UnitData", order = 0)]
    public class UnitData : ScriptableObject
    {
        [SerializeField] private int _maxHealth = 100;
        public int MaxHealth => _maxHealth;

        [SerializeField] private int _maxMana = 50;
        public int MaxMana => _maxMana;

        [SerializeField] private int _armor = 1;
        public int Armor => _armor;

        [SerializeField] private ArmorType _armorType;
        public ArmorType ArmorType => _armorType;

        [SerializeField] private float _moveSpeed = 3.5f;
        public float MoveSpeed => _moveSpeed;

        [SerializeField] private float _agressiveRadius = 3;
        public float AggressiveRadius => _agressiveRadius;
    }
}