using System;
using System.Collections.Generic;
using UnityEngine;

namespace _MainGame.Scripts.Utilities.Behaviours
{
    public static class SingletonContainer
    {
        private static readonly Dictionary<Type, object> Singletons = new();
        private static readonly Dictionary<Type, List<Action<object>>> KeepForReplaces = new();
        public static void RegistrySingleton(object content, params Type[] types)
        {
            foreach (var type in types)
            {
                if (Singletons.ContainsKey(type))
                {
                    //Debug.Log($"Replace singleton {type}");
                    Singletons.Remove(type);
                }
                Singletons.Add(type, content);
                if (!KeepForReplaces.TryGetValue(type, out var callback)) 
                    continue;
                Debug.LogWarning($"{type} has been created and will be sent to the waiters");
                KeepForReplaces.Remove(type);
                foreach (var c in callback)
                    c?.Invoke(content);
            }
        }
        public static void GetSingleton(Type type, Action<object> replaceCallback)
        {
            if (Singletons.TryGetValue(type, out var res))
            {
                replaceCallback?.Invoke(res);
                return;
            }
            
            Debug.LogWarning($"{type} could not be injected. It will be replaced when having the instance");
            if (KeepForReplaces.TryGetValue(type, out var cbList))
                cbList.Add(replaceCallback);
            else
            {
                KeepForReplaces.Add(type, new List<Action<object>>
                {
                    replaceCallback
                });
            }
        }
        public static void UnRegistrySingleton(Type type)
        {
            Singletons.Remove(type);
        }
    }
}
