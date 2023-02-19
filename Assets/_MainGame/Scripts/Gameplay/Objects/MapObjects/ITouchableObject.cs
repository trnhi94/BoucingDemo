using _MainGame.Scripts.Gameplay.Objects.DynamicObjects;
using Fusion;

namespace _MainGame.Scripts.Gameplay.Objects.MapObjects
{
    public readonly struct TouchInfo
    {
        public LagCompensatedHit Hit { get; init; }
        public float Dmg { get; init; }
    }
    public interface ITouchableObject
    {
        void HandleTouching(ITouchGiver giver, in TouchInfo info);
    }
}