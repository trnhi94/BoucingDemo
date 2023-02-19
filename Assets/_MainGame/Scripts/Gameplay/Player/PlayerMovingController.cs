using Fusion;
using UnityEngine;

namespace _MainGame.Scripts.Gameplay.Player
{
    public class PlayerMovingController : NetworkBehaviour
    { 
        [SerializeField] private float speed = 5f;
        [SerializeField] private NetworkCharacterControllerPrototype cc;

        public void Move(in Vector3 direction)
        {
            cc.Move(5*direction*speed*Runner.DeltaTime);
        }
    }
}
