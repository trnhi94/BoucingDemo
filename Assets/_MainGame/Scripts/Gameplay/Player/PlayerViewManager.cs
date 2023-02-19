using UnityEngine;

namespace _MainGame.Scripts.Gameplay.Player
{
    public class PlayerViewManager : MonoBehaviour
    {
        [field: SerializeField] public HealthView HealthView { get; private set; }
        [SerializeField] private GameObject activePart;

        public void ActivePlayer(bool active)
        {
            activePart.SetActive(active);
        }
    }
}