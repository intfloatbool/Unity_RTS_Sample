using UnityEngine;
using UnityEngine.Assertions;

namespace Units.Controllers
{
    public abstract class UnitControllerBase : MonoBehaviour
    {
        [SerializeField] protected GameUnit _gameUnit;
        public GameUnit GameUnit => _gameUnit;
        public virtual void SetUnit(GameUnit gameUnit)
        {
            _gameUnit = gameUnit;
            Assert.IsNotNull(_gameUnit, "_gameUnit != null");
        }

        protected virtual void Update()
        {
            if (_gameUnit == null)
                return;

            if (_gameUnit.IsDead)
                return;
            
            ControllUnitLoop();
        }

        protected abstract void ControllUnitLoop();

    }
}