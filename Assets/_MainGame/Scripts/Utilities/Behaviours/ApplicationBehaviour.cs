using UnityEngine;

namespace _MainGame.Scripts.Utilities.Behaviours
{
    public class ApplicationBehaviour : MonoBehaviour
    {
        public virtual void Setup()
        {
            ApplicationBehaviourUtils.SetupSingleton(this);
        }
        public virtual void LateSetup()
        {
            ApplicationBehaviourUtils.InjectComponents(this);
        }

        public void OnDestroy()
        {
            ApplicationBehaviourUtils.RemoveSingleton(this);
        }
    }
}