using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameInput
{
    public class MobileInputProvider : InputProviderBase
    {
        [SerializeField] private Joystick _joystick;
#if UNITY_EDITOR
        private readonly string VERTICAL = "Vertical";
        private readonly string HORIZONTAL = "Horizontal";
#endif
        
        private void Awake()
        {
            Assert.IsNotNull(_joystick, "_joystick != null");
        }
        public override float GetX()
        {
            if (_joystick == null)
                return 0f;
            float horValue = _joystick.Horizontal;

#if UNITY_EDITOR
            horValue += Input.GetAxis(HORIZONTAL);
#endif
            
            return horValue;
        }

        public override float GetY()
        {
            if (_joystick == null)
                return 0f;
            
            float verticalValue = _joystick.Vertical;

#if UNITY_EDITOR
            verticalValue += Input.GetAxis(VERTICAL);
#endif
            
            return verticalValue;
        }

        private void Update()
        {
            InputLoop();
        }

        private void InputLoop()
        {
            AttackInput();
            SpellInput();
        }

        private void AttackInput()
        {
            //TODO: Complete for mobile input (ui buttons)
            
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                PressButton(InputButton.ATTACK);
            }
#endif
            
        }

        private void SpellInput()
        {
            //TODO: Complete for mobile input (ui buttons)
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PressButton(InputButton.SKILL_1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PressButton(InputButton.SKILL_2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PressButton(InputButton.SKILL_3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                PressButton(InputButton.SKILL_4);
            }
#endif
        }
    }
}