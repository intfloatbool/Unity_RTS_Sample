using System;
using System.Collections.Generic;
using Game.Services.Base;
using Game.Settings;
using Units;
using Units.Properties;
using Units.Properties.Enums;
using UnityEngine;

namespace Game.Services.SingletonServices
{
    public class AttackHandler: GameServiceMonoBehaviour
    {
        [System.Serializable]
        private class AttackData
        {
            [SerializeField] private AttackType _attackType;
            public AttackType AttackType => _attackType;

            [SerializeField] private AttackSettings _attackSettings;
            public AttackSettings AttackSettings => _attackSettings;
        }

        [SerializeField] private AttackData[] _attackDataCollection;
        private Dictionary<AttackType, AttackSettings> _attackSettingDict;

        private void Awake()
        {
            InitDict();
        }
        
        

        private void InitDict()
        {
            _attackSettingDict = new Dictionary<AttackType, AttackSettings>(_attackDataCollection.Length);

            foreach (var attackData in _attackDataCollection)
            {
                if (attackData != null)
                {
                    var attackType = attackData.AttackType;
                    if (_attackSettingDict.ContainsKey(attackType))
                    {
                        Debug.LogError($"Attack Data for type {attackType}, already defined!");
                    }
                    else
                    {
                        if (attackData.AttackSettings == null)
                        {
                            Debug.LogError($"Attack data with type {attackType} property {nameof(AttackSettings)} is missing!");
                            continue;
                        }
                        _attackSettingDict.Add(attackType, attackData.AttackSettings);
                    }
                }
            }
        }

        public int NormalizeDamage(int sourceDamage, Weapon sourceWeapon, GameUnit target)
        {
            int damageToAffect = sourceDamage;

            if (sourceWeapon != null && target != null)
            {
                if (_attackSettingDict.TryGetValue(sourceWeapon.AttackType, out var attackSettings))
                {
                    foreach (var damageData in attackSettings.DamageDataForArmors)
                    {
                        if (damageData.AffectedArmorType == target.ArmorType)
                        {
                            float srcDmg = (float) sourceDamage;
                            float multipler = damageData.AttackMultipler;
                            float result = srcDmg * multipler;
                            result -= (float) target.Armor;
                            return Mathf.RoundToInt(result);
                        }
                    } 
                }
            }
            
            return damageToAffect - target.Armor;
        }
    }
}