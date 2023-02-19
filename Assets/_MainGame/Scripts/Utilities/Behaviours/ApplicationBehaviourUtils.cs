using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using _MainGame.Scripts.Utilities.Attributes;

namespace _MainGame.Scripts.Utilities.Behaviours
{
    public static class ApplicationBehaviourUtils
    {
        public static void SetupSingleton(object obj)
        {
            var type = obj.GetType();
            var singletonAtt = type.GetCustomAttribute<SingletonAttribute>();
            if (singletonAtt == null)
                return;
            var types = singletonAtt.Types;
            if (types == null)
                SingletonContainer.RegistrySingleton(obj, type);
            else
                SingletonContainer.RegistrySingleton(obj, types);
        }
        
        public static void RemoveSingleton(object obj)
        {
            var type = obj.GetType();
            var singletonAtt = type.GetCustomAttribute<SingletonAttribute>();
            if (singletonAtt == null)
                return;
            SingletonContainer.UnRegistrySingleton(type);
        }

        public static void InjectComponents(object obj)
        {
            var type = obj.GetType();
            foreach (var field in GetFieldsWithAttribute(type, typeof(InjectAttribute)))
            {
                var fieldType = field.FieldType;
                void AssignCb(object o)
                {
                    field.SetValue(obj, o);
                    var hook = field.GetCustomAttribute<InjectAttribute>().Hook;
                    if (hook is null)
                        return;
                    var hookMethod = type.GetMethod(hook, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    hookMethod?.Invoke(obj, new[] { o });
                }
                SingletonContainer.GetSingleton(fieldType, AssignCb);
            }
        }
        public static IEnumerable<FieldInfo> GetFieldsWithAttribute(Type selfType, Type attributeType)
        {
            var fields = selfType
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.GetCustomAttribute(attributeType, true) is not null);
            return fields;
        }
    }
}