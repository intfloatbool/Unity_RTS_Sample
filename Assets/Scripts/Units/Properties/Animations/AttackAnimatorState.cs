using Game.Static;
using Units.Properties.Weapons;
using UnityEngine;

namespace Units.Properties.Animations
{
    public class AttackAnimatorState : StateMachineBehaviour
    {
        private AnimationWeaponDoneController _weaponDoneController;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            if (stateInfo.IsName(GameHelper.Animations.ATTACK_STATE_NAME))
            {
                if (_weaponDoneController == null)
                {
                    _weaponDoneController = animator.GetComponent<AnimationWeaponDoneController>();
                }
                
                if (_weaponDoneController != null)
                {
                    _weaponDoneController.StartWaiting(stateInfo.length);
                }
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }
    }
}