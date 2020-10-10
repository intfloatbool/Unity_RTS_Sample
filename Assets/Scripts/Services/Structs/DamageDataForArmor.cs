using System;
using Units.Properties.Enums;
using UnityEngine;

namespace Game.Services.Structs
{
    [System.Serializable]
    public struct DamageDataForArmor: IEquatable<DamageDataForArmor>
    {
        [SerializeField] private ArmorType _affectedArmorType;
        public ArmorType AffectedArmorType => _affectedArmorType;

        [SerializeField] private float _attackMultipler;
        public float AttackMultipler => _attackMultipler;
        
        public bool Equals(DamageDataForArmor other)
        {
            return _affectedArmorType == other._affectedArmorType 
                   && Mathf.Approximately(_attackMultipler,other._attackMultipler);
        }

        public override bool Equals(object obj)
        {
            return obj is DamageDataForArmor other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _affectedArmorType.GetHashCode() ^ _attackMultipler.GetHashCode();
        }
    }
}