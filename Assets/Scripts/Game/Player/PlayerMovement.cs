using System;
using System.Collections;
using CustomDebug;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Utility;

namespace Player
{
    [Serializable]
    public class Walking : IMovement
    {
        private readonly Rigidbody rb;
        private readonly PlayerMovement.MovementVariables movementVariables;

        public Walking(Rigidbody rigidbody, PlayerMovement.MovementVariables  movementVariables)
        {
            rb = rigidbody;
            this.movementVariables = movementVariables;
        }

        public bool ReachedMaxMovementVelocity => false; // lol

        public void ApplyMovement(Vector3 movementDirection)
        {
            var velocity = CalculateVelocity(movementDirection);
            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        }

        private Vector3 CalculateVelocity(Vector3 movementDirection)
        {
            return movementDirection * movementVariables.walkValue * Time.fixedDeltaTime;
        }
    }

    [Serializable]
    public class Running : IMovement
    {
        private readonly Rigidbody rb;
        private readonly PlayerMovement.MovementVariables movementVariables;

        public Running(Rigidbody rigidbody,PlayerMovement.MovementVariables  movementVariables)
        {
            rb = rigidbody;
            this.movementVariables = movementVariables;
        }

  
        public bool ReachedMaxMovementVelocity => rb.GetHorizontalVelocity() >= movementVariables.maxRunVelocity;

        public void ApplyMovement(Vector3 movementDirection)
        {
            var movementForce = CalculateMovementForce(movementDirection);
            rb.AddForce(movementForce);
        }

        private Vector3 CalculateMovementForce(Vector3 movementDirection) 
            => movementDirection * movementVariables.runValue * Time.fixedDeltaTime;
    }

    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private MovementVariables movementVariables;
        private IInputGatherer inputGatherer;
        private IGroundedDetector groundedDetector;

        
        //  private readonly PlayerLook playerLook;
        private MovementModeProvider moveModeProvider;
        private Rigidbody rb;

        private void Awake()
        {
            this.AssignGetComponentTo<InputGatherer, IInputGatherer>(out inputGatherer);
            this.AssignGetComponentTo<GroundedDetector, IGroundedDetector>(out groundedDetector);
            this.AssignGetComponentTo(out rb);

            this.AssignGetComponentTo(out moveModeProvider);
            
            moveModeProvider.AssignDependencies(rb, movementVariables);
        }

        private void Update()
        {
            DebugText.Instance["Speed"] = $"{rb.GetHorizontalVelocity():#0.##} {rb.velocity.ToString()}";
        }

        private void FixedUpdate()
        {
            SetMovementMode();
            ApplyMovement();
        }

        private void SetMovementMode()
        {
            var sprintButtonPressed = inputGatherer.ReadShouldRun();
            moveModeProvider.UpdateMovementMode(sprintButtonPressed, rb.velocity, movementVariables);
        }

        private void ApplyMovement()
        {
            var movement = moveModeProvider.Current;
            
            if(movement.ReachedMaxMovementVelocity)
                return;

            if (groundedDetector.IsGrounded)
                ApplyGroundedMovement(movement);
            else
                ApplyAirMovement(movement);
            
        }

        private void ApplyGroundedMovement(IMovement movement)
        {
            var movementDirection = GetMovementInput();

            movement.ApplyMovement(movementDirection);
        }

        private void ApplyAirMovement(IMovement movement)
        {
            return;
        }

        private Vector3 GetMovementInput()
        {
            var movementInput = inputGatherer.ReadMovementInput();

            var movementDirection = RotateInputToLocal(movementInput);
            return movementDirection;
        }
        

        private Vector3 RotateInputToLocal(Vector3 movementInput) 
            => transform.rotation * movementInput;


        [Serializable]
        public struct MovementVariables
        {
            public float runValue;
            public float walkValue;
            public float maxRunVelocity;
            public float minRunVelocity;
        }
    }


    public static class RigidbodyExtensions
    {
        public static float GetHorizontalVelocity(this Rigidbody rb)
        {
            var horizontalMovement = rb.velocity;
            horizontalMovement.y = 0;
            return horizontalMovement.magnitude;
        }
    }
    
}