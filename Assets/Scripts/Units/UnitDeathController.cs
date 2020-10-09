using System;
using System.Collections;
using Units;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public class UnitDeathController : MonoBehaviour
    {
        [SerializeField] protected GameUnit _gameUnit;
        [SerializeField] protected float _destroyDelay = 2f;
        [SerializeField] protected bool _onlyDeactivate = true;

        private WaitForSeconds _waitForSeconds;

        private void OnValidate()
        {
            if (_gameUnit == null)
            {
                _gameUnit = GetComponent<GameUnit>();
            }
        }

        private void Awake()
        {
            _waitForSeconds = new WaitForSeconds(_destroyDelay);
            Assert.IsNotNull(_gameUnit, "_gameUnit != null");
            if (_gameUnit != null)
            {
                _gameUnit.OnDead += OnUnitDeath;
            }
        }

        private void OnDestroy()
        {
            if (_gameUnit != null)
            {
                _gameUnit.OnDead -= OnUnitDeath;
            }
        }

        protected virtual void OnUnitDeath()
        {
            StartCoroutine(RemoveBodyCoroutine());
        }

        private IEnumerator RemoveBodyCoroutine()
        {
            yield return _waitForSeconds;

            if (_onlyDeactivate)
            {
                _gameUnit.gameObject.SetActive(false);
            }
            else
            {
                Destroy(_gameUnit.gameObject);   
            }
        }
    }
}

