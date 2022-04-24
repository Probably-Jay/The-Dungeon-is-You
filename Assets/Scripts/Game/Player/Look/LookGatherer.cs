using System;
using UnityEngine;

namespace Player
{
    [Obsolete]
    internal interface ILookGatherer
    {
        float ReadMouseHorizontalDelta();
        float ReadMouseVerticalDelta();
    }
    [Obsolete]
    class LookGatherer : MonoBehaviour, ILookGatherer
    {
        [SerializeField] private bool lockMouse = true;
        
        private Vector2 mouseDelta = Vector2.zero;
        private InputManager inputManager;

        //private Vector2 lastFrameMousePosition;

        private void Awake()
        {
            inputManager = InputManager.Instance;
            
            if(!lockMouse)
                return;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            UpdateMouseDelta();
        }

        private void Update()
        {
            UpdateMouseDelta();
        }

        private void UpdateMouseDelta()
        {
            mouseDelta = inputManager.GetPlayerLookMouseDelta();
        }

        // private void GetMouse()
        // {
        //     var currentMousePosition = Input.mousePosition;
        //
        //     mouseDelta = new Vector2(currentMousePosition.x, currentMousePosition.y) - lastFrameMousePosition;
        //
        //     if (mouseDelta.magnitude < 0.01)
        //         mouseDelta = Vector2.zero;
        //
        //     lastFrameMousePosition = currentMousePosition;
        // }

        public float ReadMouseHorizontalDelta() => mouseDelta.x;

        public float ReadMouseVerticalDelta() => -mouseDelta.y;
        
        
    }
}