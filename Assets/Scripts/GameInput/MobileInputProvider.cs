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
    }
}