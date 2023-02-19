using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _MainGame.Scripts.Utilities.Scene
{
    public static class ApplicationSceneManager
    {
        public static async UniTask OperationHandling(UnityEngine.AsyncOperation operation, Action<float> callback)
        {
            if (callback is not null)
            {
                while (operation.isDone)
                {
                    callback(operation.progress);
                    await UniTask.NextFrame();
                }
            }
            else
                await operation;
        }
        public static class Load
        {
            public static async UniTask LoadSceneAsync(string scene, Action<float> callbackWhileLoading = null, bool waitForSetup = false, LoadSceneMode mode = LoadSceneMode.Additive)
            { 
                var load = SceneManager.LoadSceneAsync(scene, mode);
                await OperationHandling(load, callbackWhileLoading);
                if (waitForSetup)
                    await WaitForSetup(SceneManager.GetSceneByName(scene));
            }
            public static async UniTask LoadSceneAsync(int scene, Action<float> callbackWhileLoading = null, bool waitForSetup = false, LoadSceneMode mode = LoadSceneMode.Additive)
            {
                var load = SceneManager.LoadSceneAsync(scene, mode);
                await OperationHandling(load, callbackWhileLoading);
                if (waitForSetup)
                    await WaitForSetup(SceneManager.GetSceneByBuildIndex(scene));
            }
        }

        public static class Unload
        {
            public static UniTask UnloadSceneAsync(string scene, Action<float> callbackWhileUnloading = null, UnloadSceneOptions options = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects)
            {
                return UnloadSceneAsync(SceneManager.GetSceneByName(scene), callbackWhileUnloading, options);
            }
            public static UniTask UnloadSceneAsync(int scene, Action<float> callbackWhileUnloading = null, UnloadSceneOptions options = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects)
            {
                return UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(scene), callbackWhileUnloading, options);
            }
            public static async UniTask UnloadSceneAsync(UnityEngine.SceneManagement.Scene scene, Action<float> callbackWhileUnloading = null, UnloadSceneOptions options = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects)
            {
                var unload = SceneManager.UnloadSceneAsync(scene, options);
                await OperationHandling(unload, callbackWhileUnloading);
            }
        }

        private static async UniTask WaitForSetup(UnityEngine.SceneManagement.Scene scene)
        {
            var objs = new List<IHeavySetup>();
            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                var heavy = rootGameObject.GetComponentsInChildren<IHeavySetup>(true);
                if (heavy is not null)
                    objs.AddRange(heavy);
            }
            // var objs = scene.GetRootGameObjects()
            //     .Select(e => e.GetComponentsInParent<IHeavySetup>(true).First());
            await SetupWaiter.WaitForDoneSetup(objs);
        }
        
        public static async UniTask LoadSceneAsync(UnityEngine.SceneManagement.Scene previousScene, int newScene, 
            bool shouldActiveNewScene = true, 
            Action callbackAfterUnload = null,
            Action<float> callbackWhileLoading = null,
            Action<float> callbackWhileUnloading = null,
            bool waitForSetup = false)
        {
            await Unload.UnloadSceneAsync(previousScene, callbackWhileUnloading);
            callbackAfterUnload?.Invoke();
            await Load.LoadSceneAsync(newScene, callbackWhileLoading, waitForSetup);
            if (shouldActiveNewScene)
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(newScene));
        }
    }
}