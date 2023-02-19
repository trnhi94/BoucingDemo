using UnityEngine;

namespace _MainGame.Scripts.Gameplay.Objects.DynamicObjects
{
    public interface ITouchGiver
    {
        Vector3 CurrentDirection { get; set; }
        void ForceDestroy();
    }
}