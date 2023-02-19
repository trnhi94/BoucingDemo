using System.Linq;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _MainGame.Scripts.Utilities.Behaviours.Initialize
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField, ScenePath] private string[] scenes;
        private void Awake()
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (scenes.Contains(scene.path))
                    InitObjects(scene);
            };
        }
        private void InitObjects(UnityEngine.SceneManagement.Scene scene)
        {
            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                var initializes = rootGameObject.GetComponentsInChildren<IInitialize>(true);
                foreach(var i in initializes)
                {
                    if (!i.HasInitialized)
                        i.Initialize();
                }
            }
        }
    }
}