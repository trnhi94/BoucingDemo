using _MainGame.Scripts.Gameplay.Objects.DynamicObjects;
using Fusion;
using UnityEngine;

namespace _MainGame.Scripts.Gameplay.Player
{
    public class PlayerAttackController : NetworkBehaviour
    {
        [SerializeField] private NetworkPrefabRef prefabBall;
        [Networked] private TickTimer Delay { get; set; }
        private Transform _transform;
        
        private void Awake()
        {
            _transform = transform;
        }
        private void LookBeforeFiring(Vector3 target)
        {
            target.y = _transform.position.y;
            _transform.LookAt(target);
        }

        private void GenerateBall(in Vector3 positionInWorld)
        {
            var position = _transform.position;
            var delta = positionInWorld - position;
            delta.y = 0;
            delta.Normalize();
            Runner.Spawn(
                prefabBall,
                position + delta, 
                Quaternion.identity,
                Object.InputAuthority, 
                (_, o) =>
                {
                    if (o.TryGetBehaviour(out FlyingBall ball))
                        ball.Init(delta);
                });
        }

        public void FireBall(in Vector3 positionInWorld)
        {
            if (!Delay.ExpiredOrNotRunning(Runner)) 
                return;
            Delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
            LookBeforeFiring(positionInWorld);
            GenerateBall(positionInWorld);
        }
    }
}