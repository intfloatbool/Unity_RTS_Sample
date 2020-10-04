using System;

namespace Units.Properties.Weapons
{
    public class MeleeWeapon : Weapon
    {
        public override event Action OnWeaponUsed;
        
        protected override void UseWeapon()
        {
            
            _isReady = false;
            //TODO: Realize attack
            OnWeaponUsed?.Invoke();
        }
    }
}