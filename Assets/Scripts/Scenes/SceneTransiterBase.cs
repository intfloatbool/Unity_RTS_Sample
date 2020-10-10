using Game.Services.SingletonServices;
using Game.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public class SceneTransiterBase : MonoBehaviour
    {
        [SerializeField] protected string _sceneToGo;
        protected SceneTransitionService _transitionService;
        protected virtual void Awake()
        {
            if (GameHelper.Services != null)
            {
                _transitionService = GameHelper.Services.GetService<SceneTransitionService>();
                Assert.IsNotNull(_transitionService, "_transitionService != null");
            }
        }

        public virtual void GoToScene()
        {
            if (_transitionService != null)
            {
                _transitionService.LoadSceneByName(_sceneToGo);
            }
        }
    }
 
}
