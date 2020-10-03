using GameInput;
using UnityEngine;
using UnityEngine.Assertions;

namespace Units.Controllers
{
    public class PlayerUnitController : UnitControllerBase
    {
        [SerializeField] private InputProviderBase _inputProvider;
        [SerializeField] private float _rotationSpeed = 30f;
        
        [Space]
        [Header("Runtime")]
        [SerializeField] private Vector3 _moveDir;
        protected override void Awake()
        {
            base.Awake();
            Assert.IsNotNull(_inputProvider, "_inputProvider != null");
            Assert.IsNotNull(_gameUnit.Mover, "_gameUnit.Mover != null");
        }

        protected override void ControllUnitLoop()
        {
            if (_inputProvider == null)
                return;
            
            ControlMovementLoop();
            ControlRotationLoop();
        }

        private void ControlMovementLoop()
        {
            var mover = _gameUnit.Mover;
            if (mover == null)
            {
                return;
            }
            
            float y = _inputProvider.GetY();
            var offsetVector = _gameUnit.transform.forward * y;
            offsetVector = offsetVector * mover.MoveSpeed * Time.deltaTime;
            _moveDir = offsetVector;
            mover.MoveToByOffset(offsetVector);
        }

        private void ControlRotationLoop()
        {
            float x = _inputProvider.GetX();
            _gameUnit.transform.Rotate(Vector3.up * x * _rotationSpeed * Time.deltaTime );
        }
    }
}