using System.Collections.Generic;
using System.Threading.Tasks;
using _MainGame.Scripts.Gameplay.Main;
using _MainGame.Scripts.Utilities.Callbacks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _MainGame.Scripts.Network
{
    public readonly struct NetworkInputData : INetworkInput
    {
        public const byte Mousebutton1 = 0x01;
        public byte Buttons { get; init; }
        public Vector3 Direction { get; init; }
        public Vector3 MousePositionInWorldSpace { get; init; }
    }

    public class NetworkManager : BaseMonoCallbacksBehaviour   
    {
        [SerializeField] private InputPoller inputPoller;
        //private readonly Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new();
        //[SerializeField] private Vector3[] spawnPoints;
        private NetworkRunner _runner;
        
        private void OnGUI()
        {
            if (_runner == null)
            {
                if (GUI.Button(new Rect(0,0,200,40), "Host"))
                {
                    StartGame(GameMode.Host);
                }
                if (GUI.Button(new Rect(0,40,200,40), "Join"))
                {
                    StartGame(GameMode.Client);
                }
            }
        }

        private async Task EndGame()
        {
            if (_runner && !_runner.IsShutdown)
            {
                await _runner.Shutdown();
                Destroy(_runner);
            }
            _runner = null;
        }
        private NetworkRunner AddRunner()
        {
            var runner = gameObject.AddComponent<NetworkRunner>();
            runner.ProvideInput = true;
            runner.AddCallbacks(inputPoller);
            runner.AddCallbacks(this);
            return runner;
        }
        
        private async void StartGame(GameMode mode)
        {
            if (_runner)
                await EndGame();
            _runner = AddRunner();
            await _runner.StartGame(new StartGameArgs
            {
                GameMode = mode,
                SessionName = "TestRoom",
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
            //_runner.RegisterSceneObjects(walls);
        }
        // public override void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        // {
        //     if (runner.IsServer)
        //     {
        //         // Create a unique position for the player
        //         NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, spawnPoints[player % 2], Quaternion.identity, player);
        //         // Keep track of the player avatars so we can remove it when they disconnect
        //         _spawnedCharacters.Add(player, networkPlayerObject);
        //     }
        // }
        //
        // public override void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        // {
        //     // Find and remove the players avatar
        //     if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        //     {
        //         runner.Despawn(networkObject);
        //         _spawnedCharacters.Remove(player);
        //     }
        // }
    }
}