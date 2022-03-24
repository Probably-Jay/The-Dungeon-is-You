using System;
using UnityEngine;

namespace Player
{
    internal interface ILookGatherer
    {
        float ReadMouseXDelta();
        float ReadMouseYDelta();
    }
    class LookGatherer : MonoBehaviour, ILookGatherer
    {
        [SerializeField] private bool lockMouse = true;
        
        private Vector2 mouseDelta = Vector2.zero;

        private Vector2 lastFrameMousePosition;

        private void Awake()
        {
            if(!lockMouse)
                return;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        private void GetMouse()
        {
            var currentMousePosition = Input.mousePosition;

            mouseDelta = new Vector2(currentMousePosition.x, currentMousePosition.y) - lastFrameMousePosition;

            if (mouseDelta.magnitude < 0.01)
                mouseDelta = Vector2.zero;

            lastFrameMousePosition = currentMousePosition;
        }

        public float ReadMouseXDelta() => mouseDelta.x;

        public float ReadMouseYDelta() => mouseDelta.y;
    }
}