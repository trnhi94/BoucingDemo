using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using _MainGame.Scripts.Gameplay.Player;
using _MainGame.Scripts.Utilities.Attributes;
using _MainGame.Scripts.Utilities.Behaviours;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace _MainGame.Scripts.Gameplay.Main
{
    public enum GameState
    {
        Waiting, Running, WaitingToEndRound, End
    }
    
    [Singleton]
    public class GameplayController : NetworkApplicationBehaviour
    {
        [SerializeField] private float timeToEndRound;
        [SerializeField] private float delayGameStart;
        [SerializeField] private PlayerSpawnManager spawnManager;
        [Inject] private GameplayViewManager _viewManager;
        [Networked] public GameState GameState { get; set; }
        [Networked(OnChanged = nameof(OnDelayChanged))] private float CurrentDelayRemainTime { get; set; }
        public bool IsRunning => Object != null && GameState == GameState.Running;
        private readonly WaitForSeconds _waitOneSec = new(1f);
        public PlayerRef CurrentActivePlayer
        {
            get
            {
                if (CurrentActivePlayerIndex < 0 || CurrentActivePlayerIndex >= _currentRefs.Length)
                    return PlayerRef.None;
                return _currentRefs[CurrentActivePlayerIndex];
            }
        }
        [Networked(OnChanged = nameof(OnPlayerActiveIndexChanged))] private int CurrentActivePlayerIndex { get; set; }
        private Dictionary<PlayerRef, PlayerMainController> MainControllers { get; } = new();
        private PlayerRef[] _currentRefs;
        
        public override void Spawned()
        {
            base.Spawned();
            if (!HasStateAuthority)
                return;
            StartCoroutine(StartGameCoroutine());
        }

        public static void OnDelayChanged(Changed<GameplayController> changed)
        {
            changed.Behaviour.UpdateDelay();
        }
        
        public static void OnPlayerActiveIndexChanged(Changed<GameplayController> changed)
        {
            changed.Behaviour.UpdateActivePlayer();
        }

        private void UpdateActivePlayer()
        {
            _viewManager.ActivePlayer(CurrentActivePlayerIndex);
        }

        private void UpdateDelay()
        {
            _viewManager.WaitingView.SetDelay(CurrentDelayRemainTime);
        }

        private readonly SemaphoreSlim _endRoundSem = new(1);
        public async void PlayerEndRound()
        {
            if (_endRoundSem.CurrentCount == 0)
            {
                Debug.LogWarning("Conflict when handling end round");
                return;
            }
            await _endRoundSem.WaitAsync();
            GameState = GameState.WaitingToEndRound;
            _isInRound = false;
            PlayerStartRound();
            _endRoundSem.Release();
        }

        private CancellationTokenSource _cancellation;
        private bool _isInRound;
        private bool _isHandlingEndRound;
        private void PlayerStartRound()
        {
            //Next player => but not out of range
            CurrentActivePlayerIndex = ++CurrentActivePlayerIndex % _currentRefs.Length;
            CurrentDelayRemainTime = timeToEndRound;
            GameState = GameState.Running;
            StartCoroutine(WaitForEndRoundCoroutine());
        }

        private IEnumerator WaitForEndRoundCoroutine()
        {
            _isInRound = true;
            //_donePreviousRound = false;
            while (_isInRound && CurrentDelayRemainTime > 0)
            {
                CurrentDelayRemainTime--;
                yield return _waitOneSec;
            }
            if (_isInRound)
                PlayerEndRound();
            //_donePreviousRound = true;
        }

        private IEnumerator StartGameCoroutine()
        {
            CurrentDelayRemainTime = delayGameStart;
            foreach (var mainController in spawnManager.SpawnPlayers(Runner))
                MainControllers.Add(mainController.Object.InputAuthority, mainController);
            _currentRefs = MainControllers.Keys.ToArray();
            while (CurrentDelayRemainTime > 0)
            {
                CurrentDelayRemainTime--;
                yield return _waitOneSec;
            }
        }
    }
}