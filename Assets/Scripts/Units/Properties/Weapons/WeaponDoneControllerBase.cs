using System;
using UnityEngine;

namespace Units.Properties.Weapons
{
    public abstract class WeaponDoneControllerBase : MonoBehaviour
    {
        protected bool _isWaitForDone = false;
        public bool IsWaitForDone => _isWaitForDone;
        public abstract void WaitForAttackDone(Weapon weapon, Action callBack);
    }
}