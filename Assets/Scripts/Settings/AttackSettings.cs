using System.Collections.Generic;
using Game.Services.Structs;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "AttackSettings", menuName = "Settings/AttackSettings", order = 0)]
    public class AttackSettings : ScriptableObject
    {
        [SerializeField] private DamageDataForArmor[] _damageDataForArmors;
        public IReadOnlyCollection<DamageDataForArmor> DamageDataForArmors => _damageDataForArmors;
    }
}