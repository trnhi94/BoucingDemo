using _MainGame.Scripts.Gameplay.Objects.DynamicObjects;
using Fusion;
using UnityEngine;

namespace _MainGame.Scripts.Gameplay.Objects.MapObjects
{
    public class Bouncer : NetworkBehaviour, ITouchableObject
    {
        public void HandleTouching(ITouchGiver giver, in TouchInfo info)
        {
            var reflectDir = Vector3.Reflect(giver.CurrentDirection, info.Hit.Normal);
            //Debug.Log($"{giver.CurrentDirection} bounced {hit.Point} to: {reflectDir}");
            giver.CurrentDirection = reflectDir;
        }
    }
}