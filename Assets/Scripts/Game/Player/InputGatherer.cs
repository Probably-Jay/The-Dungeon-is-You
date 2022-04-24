using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Player
{
    public interface IInputGatherer 
    {
        Vector3 ReadMovementInput();
        bool ReadShouldJump();
        bool ReadShouldRun();
    }

    public class InputGatherer : MonoBehaviour, IInputGatherer
    {
        private Vector3 movementInput = Vector2.zero;
        private ButtonInput jumpInput;
        private ButtonInput runInput;
        private InputManager inputManger;


        private void Awake()
        {
            inputManger = InputManager.Instance;
            jumpInput = new ButtonInput(inputManger.GetPlayerJumpButtonDown);
            runInput = new ButtonInput(inputManger.GetPlayerSprintButtonDown);
        }

        private void Update()
        {
            movementInput = GetMovementDirection();
            jumpInput.SetButtonValue();
            runInput.SetButtonValue();
        }

        private Vector3 GetMovementDirection()
        {
            var movement = inputManger.GetPlayerMovement();
            var horizontal = movement.x;
            var vertical = movement.y;

            return new Vector3(horizontal,0, vertical).normalized;
        }
        
        public Vector3 ReadMovementInput() => movementInput;

        public bool ReadShouldJump() => jumpInput.ReadAndReset();

        public bool ReadShouldRun() => runInput.ReadAndReset();


        /// <summary>
        /// Helper class to read a value and reset it when read.
        /// Ensures input is not missed between Update and FixedUpdate
        /// </summary>
        private class ButtonInput
        {
            private bool value;
            private readonly Func<bool> isKeyDown;

            public ButtonInput([NotNull] Func<bool> keyDown)
            {
                this.isKeyDown = keyDown;
                value = false;
            }

            public void SetButtonValue()
            {
                if (value) // do not reset until read
                    return;

                value = isKeyDown();
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