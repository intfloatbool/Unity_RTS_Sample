using System;
using Units;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace MyNamespace
{
    public class UnitHpBarVisual : MonoBehaviour
    {
        [SerializeField] private Image _hpBarImg;
        [SerializeField] private GameUnit _gameUnit;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private Vector3 _unitOffset;

        private void Awake()
        {
            if (_gameUnit != null)
            {
                SetUnit(_gameUnit);
            }
        }

        public void SetUnit(GameUnit gameUnit)
        {
            _gameUnit = gameUnit;
            var navAgent = gameUnit.GetComponent<NavMeshAgent>();
            if (navAgent != null)
            {
                _unitOffset = Vector3.up * navAgent.height;
            }
        }

        private Vector3 GetPositionByUnit()
        {
            return _gameUnit.transform.position + _unitOffset;
        }

        private void Update()
        {
            if (_hpBarImg == null)
                return;
            if (_gameUnit == null)
                return;
            
            transform.position = GetPositionByUnit();
            _hpBarImg.gameObject.SetActive(!_gameUnit.IsDead);
            _hpBarImg.fillAmount = _gameUnit.NormalizedHealth;
            
            if (_gradient != null)
            {
                _hpBarImg.color = _gradient.Evaluate(_hpBarImg.fillAmount);
            }
        }
    }
}
