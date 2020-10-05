using System;
using UnityEngine;

namespace Units.Properties.Weapons
{
    public abstract class WeaponDoneControllerBase : MonoBehaviour
    {
        public abstract void WaitForAttackDone(Weapon weapon, Action callBack);
    }
}