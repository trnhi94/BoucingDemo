using _MainGame.Scripts.Gameplay.Objects.MapObjects;
using Fusion;
using UnityEngine;

namespace _MainGame.Scripts.Gameplay.Objects.DynamicObjects
{
    public class FlyingBall : NetworkBehaviour, ITouchGiver
    {
        [Networked] public Vector3 CurrentDirection { get; set; }
        [SerializeField] private float speed;
        // [Networked] public TickTimer DelayBouncingTimer { get; set; }
        // private const float DelayBouncingTime = 1f;
        [SerializeField] private float radius;
        [SerializeField] private LayerMask hitLayers;
        //private int _targetLayers;
        [SerializeField] private Transform cacheTransform;
        //private readonly List<LagCompensatedHit> _hitsToWall = new();
        private LagCompensatedHit _hit;
        
        public void Init(Vector3 initDir)
        {
            CurrentDirection = initDir;
            //_targetLayers = PhysicsUtilities.GetMask(hitLayers);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(cacheTransform.position, radius);
        }

        private bool CheckTouchTarget()
        {
            return Runner.LagCompensation.Raycast(
                cacheTransform.position,
                CurrentDirection,
                radius,
                Object.InputAuthority,
                out _hit,
                hitLayers.value,
                HitOptions.IgnoreInputAuthority | HitOptions.IncludePhysX);
        }

        private void HandleTarget()
        {
            if (_hit.Collider.TryGetComponent(out ITouchableObject obj))
                obj.HandleTouching(this, new TouchInfo
                {
                    Hit = _hit
                });
        }

        public void ForceDestroy()
        {
            
        }
        
        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority)
                return;
            if (CheckTouchTarget())
            {
                //DelayBouncingTimer = TickTimer.CreateFromSeconds(Runner,DelayBouncingTime);
                HandleTarget();
            }
            else
            {
                transform.Translate(CurrentDirection * Runner.DeltaTime * speed, Space.World);
            }
        }
    }
}