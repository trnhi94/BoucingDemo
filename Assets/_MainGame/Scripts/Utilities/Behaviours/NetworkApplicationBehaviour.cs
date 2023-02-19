using Fusion;

namespace _MainGame.Scripts.Utilities.Behaviours
{
    public class NetworkApplicationBehaviour : NetworkBehaviour
    {
        public void SetupSingleton()
        {
            ApplicationBehaviourUtils.SetupSingleton(this);
        }

        public void InjectComponents()
        {
            ApplicationBehaviourUtils.InjectComponents(this);
        }
        public override void Spawned()
        {
            SetupSingleton();
            InjectComponents();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            ApplicationBehaviourUtils.RemoveSingleton(this);
        }
    }
}