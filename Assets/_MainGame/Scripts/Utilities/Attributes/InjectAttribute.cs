using System;

namespace _MainGame.Scripts.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    {
        public string Hook { get; }

        public InjectAttribute()
        {
            
        }
        public InjectAttribute(string hook)
        {
            this.Hook = hook;
        }
    }
}