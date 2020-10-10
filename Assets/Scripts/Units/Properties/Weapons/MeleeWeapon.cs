namespace Units.Properties.Weapons
{
    public class MeleeWeapon : Weapon
    {
        protected override void UseWeapon()
        {
            _isReady = false;
        }

        protected override void WeaponUsed()
        {
            if (Owner != null)
            {
                DamageToTargetNormalized();
            }
            
            base.WeaponUsed();
        }
    }
}