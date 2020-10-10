using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Services.Base;
using Game.Services.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Services.SingletonServices
{
    public class SceneTransitionService : GameServiceMonoBehaviour
    {
        [System.Serializable]
        private class TypedSceneName
        {
            [SerializeField] private string _sceneName;
            public string SceneName => _sceneName;

            [SerializeField] private SceneType _sceneType;
            public SceneType SceneType => _sceneType;
        }

        [SerializeField] private TypedSceneName[] _typedSceneNamesCollection;
        
        [Space]
        [SerializeField] private float _delayAfterLoad;

        private Dictionary<string, TypedSceneName> _typedNamesDict;
        private WaitForSecondsRealtime _delayWaiting;
        private Coroutine _switchCoroutine;
        
        public event Action<string> OnSceneStartLoading;
        public event Action<string> OnSceneLoaded;
        
        public event Action<SceneType> OnSceneStartLoadingByType;
        public event Action<SceneType> OnSceneLoadedByType;

        private void Awake()
        {
            _delayWaiting = new WaitForSecondsRealtime(_delayAfterLoad);

            _typedNamesDict = _typedSceneNamesCollection.ToDictionary(tn => tn.SceneName);
        }

        public void LoadSceneByName(string sceneName)
        {
            if (_switchCoroutine != null)
            {
                Debug.LogError("Some loading process not done.");
                return;
            }

            _switchCoroutine = StartCoroutine(LoadSceneByNameCoroutine(sceneName));
        }

        private IEnumerator LoadSceneByNameCoroutine(string sceneName)
        {
            TypedSceneName typedSceneName = null;
            _typedNamesDict.TryGetValue(sceneName, out typedSceneName);

            OnSceneStartLoading?.Invoke(sceneName);
            if (typedSceneName != null)
            {
                OnSceneStartLoadingByType?.Invoke(typedSceneName.SceneType);
            }
            
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            while (!asyncOperation.isDone)
            {
                yield return null;
            }

            if (!Mathf.Approximately(_delayAfterLoad, 0f))
            {
                yield return _delayWaiting;
            }
            
            yield return null;
            
            _switchCoroutine = null;
            
            OnSceneLoaded?.Invoke(sceneName);
            if (typedSceneName != null)
            {
                OnSceneLoadedByType?.Invoke(typedSceneName.SceneType);
            }
            
            yield return null;
        }
    }
}