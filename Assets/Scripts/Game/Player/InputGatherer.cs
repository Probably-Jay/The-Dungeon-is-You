using System;
using UnityEngine;

namespace Player
{
    public interface IInputGatherer 
    {
        Vector3 ReadMovementInput();
        bool ReadShouldJump();
        bool ReadShouldRun();
    }

    internal class InputGatherer : MonoBehaviour, IInputGatherer
    {
        private Vector3 movementInput = Vector2.zero;
        private readonly ButtonInput jumpInput = new ButtonInput(KeyCode.Space);
        private readonly ButtonInput runInput = new ButtonInput(KeyCode.LeftShift);
        
        private void Update()
        {
            movementInput = GetMovementDirection();
            jumpInput.SetButtonValue();
            runInput.SetButtonValue();
        }

        private static Vector3 GetMovementDirection()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            return new Vector3(horizontal,0, vertical).normalized;
        }
        
        public Vector3 ReadMovementInput() => movementInput;

        public bool ReadShouldJump() => jumpInput.ReadAndReset();

        public bool ReadShouldRun() => runInput.ReadAndReset();


        private class ButtonInput
        {
            private bool value;
            private readonly KeyCode button;

            public ButtonInput(KeyCode button)
            {
                this.button = button;
                value = false;
            }

            public void SetButtonValue()
            {
                if (value) // do not reset until read
                    return;

                value = Input.GetKeyDown(button);
            }

            public bool ReadAndReset()
            {
                try // after return effect
                {
                    return value;
                }
                finally
                {
                    value = false; // set to false as it has been read
                }
            }
        }
        
    }
}