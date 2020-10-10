using System.Collections.Generic;
using Game.Services;
using Units.Enums;
using UnityEngine;

namespace Game.Static
{
    public static class GameHelper
    {
        //TODO: Make it false to remove all test calls.
        public const bool IS_TESTING = true;
        
        
        public static GameServices Services => GameServices.Instance;

        public static class GameTags
        {
            public const string GAME_UNIT = "GameUnit";
        }
        
        public static class Animations
        {

            public const string STAND_STATE_NAME = "STAND";
            public const string MOVE_STATE_NAME = "MOVE";
            public const string ATTACK_STATE_NAME = "ATTACK";
            
            public const string MOVE_DIR_PARAM = "MOVE_DIR";
            
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
                        animator.SetBool(_animationsDictHashes[IS_ATTACK_KEY], true);
                        break;
                    }
                }
            }
        }
    }
}
