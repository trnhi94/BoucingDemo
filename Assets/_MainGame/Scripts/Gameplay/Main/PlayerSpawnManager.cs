using System.Collections.Generic;
using _MainGame.Scripts.Gameplay.Player;
using Fusion;
using UnityEngine;

namespace _MainGame.Scripts.Gameplay.Main
{
    public class PlayerSpawnManager : MonoBehaviour
    {
        [SerializeField] private NetworkPrefabRef playerPrefab;
        [SerializeField] private Vector3[] positions;

        public IEnumerable<PlayerMainController> SpawnPlayers(NetworkRunner runner)
        {
            foreach (var playerRef in runner.ActivePlayers)
            {
                var viewIndex = playerRef % positions.Length;
                var player = runner.Spawn(playerPrefab,
                    positions[viewIndex],
                    Quaternion.identity,
                    playerRef, (_, o) =>
                    {
                        if (o.TryGetBehaviour(out PlayerMainController mainController))
                        {
                            mainController.Init(viewIndex, 3f);
                        }
                    });
                yield return player.GetBehaviour<PlayerMainController>();
            }
        }
    }
}