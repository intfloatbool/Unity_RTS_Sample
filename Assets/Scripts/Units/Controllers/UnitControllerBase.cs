using UnityEngine;
using UnityEngine.Assertions;

namespace Units.Controllers
{
    public abstract class UnitControllerBase : MonoBehaviour
    {
        [SerializeField] protected GameUnit _gameUnit;

        protected virtual void Awake()
        {
            Assert.IsNotNull(_gameUnit, "_gameUnit != null");
        }

        protected virtual void Update()
        {
            if (_gameUnit == null)
                return;
            ControllUnitLoop();
        }

        protected abstract void ControllUnitLoop();

    }
}