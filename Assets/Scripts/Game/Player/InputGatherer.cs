using System;
using UnityEngine;

namespace Player
{
    public interface IInputGatherer 
    {
        Vector3 ReadMovementInput();
        bool ReadShouldJump();
    }

    internal class InputGatherer : MonoBehaviour, IInputGatherer
    {
        private Vector3 movementInput = Vector2.zero;
        private bool jumpInput = false;
        
        private void Update()
        {
            movementInput = GetMovementDirection();
            jumpInput = GetJumpInput();
        }

        private static Vector3 GetMovementDirection()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            return new Vector3(horizontal,0, vertical).normalized;
        }

        private bool GetJumpInput()
        {
            if (jumpInput)
                return jumpInput;

            var shouldJump = Input.GetKeyDown(KeyCode.Space);
            return shouldJump;
        }

        public Vector3 ReadMovementInput()
        {
            return movementInput;
        } 
        
        public bool ReadShouldJump()
        {
            try
            {
                return jumpInput;
            }
            finally
            {
                jumpInput = false; // set to false as it has been read
            }
        }

    
    }
}