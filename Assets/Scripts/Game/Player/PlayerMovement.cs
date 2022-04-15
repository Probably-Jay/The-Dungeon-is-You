using System;
using System.Collections;
using UnityEngine;
using Utility;

namespace Player
{
    class Walking : IMovement
    {
        private readonly Rigidbody rb;
        private readonly PlayerMovement.MovementVariables movementVariables;

        public Walking(Rigidbody rigidbody, PlayerMovement.MovementVariables  movementVariables)
        {
            rb = rigidbody;
            this.movementVariables = movementVariables;
        }

        public bool ReachedMaxMovementVelocity => false;

        public void ApplyMovement(Vector3 movementDirection)
        {
            var velocity = CalculateVelocity(movementDirection);
            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        }

        private Vector3 CalculateVelocity(Vector3 movementDirection)
        {
            return movementDirection * movementVariables.walkSpeed * Time.fixedDeltaTime;
        }
    }

    public class Running : IMovement
    {
        private readonly Rigidbody rb;
        private readonly PlayerMovement.MovementVariables movementVariables;

        public Running(Rigidbody rigidbody,PlayerMovement.MovementVariables  movementVariables)
        {
            rb = rigidbody;
            this.movementVariables = movementVariables;
        }

        public bool ReachedMaxMovementVelocity => rb.velocity.magnitude >= movementVariables.maxRunVelocity;

        public void ApplyMovement(Vector3 movementDirection)
        {
            var movementForce = CalculateMovementForce(movementDirection);
            rb.AddForce(movementForce);
        }

        private Vector3 CalculateMovementForce(Vector3 movementDirection) 
            => movementDirection * movementVariables.runSpeed * Time.fixedDeltaTime;
    }

    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private MovementVariables movementVariables;
        private IInputGatherer inputGatherer;
        private IGroundedDetector groundedDetector;

        
        //  private readonly PlayerLook playerLook;
        private MovementModeProvider moveModeProvider;

        private void Awake()
        {
            this.AssignGetComponentTo<InputGatherer, IInputGatherer>(out inputGatherer);
            this.AssignGetComponentTo<GroundedDetector, IGroundedDetector>(out groundedDetector);
            this.AssignGetComponentTo(out Rigidbody rb);

            moveModeProvider = new MovementModeProvider(rb, movementVariables);
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }

        private void ApplyMovement()
        {
            var movement = moveModeProvider.Current;
            
            if(movement.ReachedMaxMovementVelocity)
                return;

            if (!groundedDetector.CanWalk)
                return;

            var movementDirection = GetMovementInput();
            
            movement.ApplyMovement(movementDirection);
        }

        private Vector3 GetMovementInput()
        {
            var movementInput = inputGatherer.ReadMovementInput();

            var movementDirection = RotateInputToLocal(movementInput);
            return movementDirection;
        }

      


        private Vector3 RotateInputToLocal(Vector3 movementInput) 
            => transform.rotation * movementInput;

        public void SetDependencies(IInputGatherer inputGathererIn, IGroundedDetector groundDetectorIn)
        {
            inputGatherer = inputGathererIn;
            groundedDetector = groundDetectorIn;
        }

       

        [Serializable]
        public class MovementVariables
        {
            public float runSpeed;
            public float walkSpeed;
            public float maxRunVelocity;
      //      public float maxWalkVelocity;
            public float jumpForce;
          
        }
    }
}