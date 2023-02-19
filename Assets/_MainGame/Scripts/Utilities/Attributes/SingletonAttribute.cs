using System;

namespace _MainGame.Scripts.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonAttribute : Attribute
    {
        public Type[] Types { get; }
        public SingletonAttribute()
        {
            Types = null;
        }
        public SingletonAttribute(params Type[] types)
        {
            Types = types;
        }
    }
}