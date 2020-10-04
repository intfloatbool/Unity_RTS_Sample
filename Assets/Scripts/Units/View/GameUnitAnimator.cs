using Game.Static;
using Units.Enums;
using UnityEngine;
using UnityEngine.Assertions;

namespace Units.View
{
    public class GameUnitAnimator : MonoBehaviour
    {
        [SerializeField] private GameUnit _gameUnit;
        [SerializeField] private Animator _unitAnimator;

        private void Awake()
        {
            Assert.IsNotNull(_unitAnimator ,"_unitAnimator != null");
            Assert.IsNotNull(_gameUnit, "_gameUnit != null");

            if (_gameUnit != null)
            {
                _gameUnit.OnStateChanged += OnUnitStateChanged;
            }
        }

        private void OnDestroy()
        {
            if (_gameUnit != null)
            {
                _gameUnit.OnStateChanged -= OnUnitStateChanged;
            }
        }

        private void OnUnitStateChanged(UnitState unitState)
        {
            if (_unitAnimator == null)
                return;
            
            GameHelper.Animations.SetUnitAnimatorFlagByState(_unitAnimator, unitState);
        }
        
    }
}