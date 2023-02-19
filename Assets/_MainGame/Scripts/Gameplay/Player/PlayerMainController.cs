using _MainGame.Scripts.Gameplay.Main;
using _MainGame.Scripts.Network;
using _MainGame.Scripts.Utilities.Attributes;
using _MainGame.Scripts.Utilities.Behaviours;
using Fusion;
using UnityEngine;

namespace _MainGame.Scripts.Gameplay.Player
{
    public class PlayerMainController : NetworkApplicationBehaviour
    {
        [field: SerializeField] public PlayerMovingController MovingController { get; private set; }
        [field: SerializeField] public PlayerAttackController AttackController { get; private set; }
        [field: SerializeField] public PlayerDefenseController DefenseController { get; private set; }
        [Inject] private GameplayController _gameplayController;
        [Inject] private GameplayViewManager _viewManager;
        [Networked] public int PlayerViewIndex { get; set; }
        public override void Spawned()
        {
            InjectComponents();
            var view = _viewManager.PlayerViews[PlayerViewIndex];
            DefenseController.ViewManager = view;
        }
        public void Init(int viewIndex, float health)
        {
            PlayerViewIndex = viewIndex;
            DefenseController.Init(health);
        }
        
        public override void FixedUpdateNetwork()
        {
            if (Object == null ||
                !_gameplayController.IsRunning ||
                _gameplayController.CurrentActivePlayer != Object.InputAuthority)
                return;
            if (!GetInput(out NetworkInputData data)) 
                return;
            MovingController.Move(data.Direction.normalized);
            if ((data.Buttons & NetworkInputData.Mousebutton1) != 0)
            {
                AttackController.FireBall(data.MousePositionInWorldSpace);
                if (HasStateAuthority)
                    _gameplayController.PlayerEndRound();
            }
        }
    }
}