using _MainGame.Scripts.Gameplay.Player;
using _MainGame.Scripts.Utilities.Attributes;
using UnityEngine;

namespace _MainGame.Scripts.Gameplay.Main
{
    [Singleton]
    public class GameplayViewManager : MonoBehaviour
    {
        [field: SerializeField] public PlayerViewManager[] PlayerViews { get; private set; }
        [field: SerializeField] public WaitStartGameView WaitingView { get; private set; }

        public void ActivePlayer(int index)
        {
            foreach (var playerView in PlayerViews)
                playerView.ActivePlayer(false);
            if (index < PlayerViews.Length)
                PlayerViews[index].ActivePlayer(true);
        }
    }
}