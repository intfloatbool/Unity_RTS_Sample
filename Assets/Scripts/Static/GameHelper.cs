using System;
using System.Collections.Generic;
using Units.Enums;
using UnityEngine;

namespace Game.Static
{
    public static class GameHelper
    {
        public static class Animations
        {

            private const string IS_MOVE_KEY = "IS_MOVE";
            private const string IS_ATTACK_KEY = "IS_ATTACK";

            private static Dictionary<string, int> _animationsDictHashes = new Dictionary<string, int>()
            {
                { IS_MOVE_KEY, Animator.StringToHash(IS_MOVE_KEY) },
                { IS_ATTACK_KEY, Animator.StringToHash(IS_ATTACK_KEY) }
            };
            
            public static void SetUnitAnimatorFlagByState(Animator animator, UnitState unitState)
            {
                switch (unitState)
                {
                    case UnitState.STANDING:
                    {
                        animator.SetBool(_animationsDictHashes[IS_MOVE_KEY], false);
                        animator.SetBool(_animationsDictHashes[IS_ATTACK_KEY], false);
                        break;
                    }
                    case UnitState.MOVING:
                    {
                        animator.SetBool(_animationsDictHashes[IS_MOVE_KEY], true);
                        animator.SetBool(_animationsDictHashes[IS_ATTACK_KEY], false);
                        break;
                    }
                    case UnitState.ATTACKING:
                    {
                        animator.SetBool(_animationsDictHashes[IS_MOVE_KEY], false);
                        animator.SetBool(_animationsDictHashes[IS_ATTACK_KEY], false);
                        break;
                    }
                }
            }
        }
    }
}
