using Game.Services;
using Game.Services.SingletonServices;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public class StartupSceneTransiter : SceneTransiterBase
    {
        [SerializeField] private GameServices _gameServices;
        
        protected override void Awake()
        {
            Assert.IsNotNull(_gameServices, "_gameServices != null");
            if (_gameServices != null)
            {
                _gameServices.OnServiceLoadingDone += OnServicesLoaded;
            }
        }

        private void OnDestroy()
        {
            if (_gameServices != null)
            {
                _gameServices.OnServiceLoadingDone -= OnServicesLoaded;
            }
        }

        private void OnServicesLoaded()
        {
            _transitionService = _gameServices.GetService<SceneTransitionService>();
            Assert.IsNotNull(_transitionService, "_transitionService != null");

            GoToScene();
        }
    }
}

