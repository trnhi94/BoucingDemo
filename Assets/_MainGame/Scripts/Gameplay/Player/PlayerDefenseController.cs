using _MainGame.Scripts.Gameplay.Objects.DynamicObjects;
using _MainGame.Scripts.Gameplay.Objects.MapObjects;
using Fusion;

namespace _MainGame.Scripts.Gameplay.Player
{
    public class PlayerDefenseController : NetworkBehaviour, ITouchableObject
    {
        [Networked(OnChanged = nameof(OnHealthChanged))] public float Health { get; set; }
        //[Networked(OnChanged = nameof(OnHealthChanged))] public NetworkBool IsDead { get; set; }
        [Networked] public float MaxHealth { get; set; }
        public PlayerViewManager ViewManager { get; set; }
        
        public void Init(float initHealth)
        {
            MaxHealth = Health = initHealth;
        }
        
        public static void OnHealthChanged(Changed<PlayerDefenseController> changed)
        {
            changed.Behaviour.UpdateHealth();
        }
        
        private void UpdateHealth()
        {
            ViewManager.HealthView.SetHealth(Health, MaxHealth);
        }
        private void HandleTakeDmg(in float dmg)
        {
            
        }
        
        private void HandleDead()
        {
            
        }

        public void HandleTouching(ITouchGiver giver, in TouchInfo info)
        {
            var dmg = info.Dmg;
            if (Health - dmg < 0)
                HandleTakeDmg(dmg);
            else
                HandleDead();
        }
    }
}