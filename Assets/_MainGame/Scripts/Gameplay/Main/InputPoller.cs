using _MainGame.Scripts.Network;
using _MainGame.Scripts.Utilities.Callbacks;
using Fusion;
using UnityEngine;

namespace _MainGame.Scripts.Gameplay.Main
{
    public class InputPoller : BaseMonoCallbacksBehaviour
    {
        private bool _mouseButton0;
        private void Update()
        {
            _mouseButton0 |= Input.GetMouseButton(0);
        }
        
        private static Vector3 GetMoveDirection()
        {
            Vector3 direction = default;
            if (Input.GetKey(KeyCode.W))
                direction += Vector3.forward;
            if (Input.GetKey(KeyCode.S))
                direction += Vector3.back;
            if (Input.GetKey(KeyCode.A))
                direction += Vector3.left;
            if (Input.GetKey(KeyCode.D))
                direction += Vector3.right;
            return direction;
        }

        private static Vector3 GetMousePositionInWorldSpace()
        {
            var mousePosInScreen = Input.mousePosition;
            return Camera.main.ScreenToWorldPoint(mousePosInScreen);
        }
        
        public override void OnInput(NetworkRunner runner, NetworkInput input)
        {
            byte btn = default;
            if (_mouseButton0)
                btn |= NetworkInputData.Mousebutton1;
            var data = new NetworkInputData
            {
                Direction = GetMoveDirection(),
                Buttons =  btn,
                MousePositionInWorldSpace = GetMousePositionInWorldSpace()
            };
            _mouseButton0 = false;
            input.Set(data);
        }
    }
}