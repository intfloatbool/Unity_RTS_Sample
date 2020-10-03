using System;
using UnityEngine;

namespace GameInput
{
    public abstract class InputProviderBase : MonoBehaviour
    {
        public event Action<InputButton> OnButtonPressed; 
        
        public abstract float GetX();
        public abstract float GetY();

        protected virtual void PressButton(InputButton inputButton)
        {
            OnButtonPressed?.Invoke(inputButton);
        }
    }
}