using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerMovement.MovementVariables movementVariables;
        private IInputGatherer inputGatherer;
        private IGroundedDetector groundedDetector;
        private Rigidbody rb;
        private readonly PlayerLook playerLook;
        

        private void Awake()
        {
            this.AssignGetComponentTo<InputGatherer, IInputGatherer>(out inputGatherer);
            this.AssignGetComponentTo<GroundedDetector, IGroundedDetector>(out groundedDetector);
            this.AssignGetComponentTo(out rb);
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }

        private void ApplyMovement()
        {
            if(rb.velocity.magnitude > movementVariables.maxVelocity)
                return;

            if (!groundedDetector.CanWalk)
                return;

            var movementInput = inputGatherer.ReadMovementInput();

            var movementDirection = RotateInputToLocal(movementInput);
            
            if (movementDirection == Vector3.zero)
                return;

            var movementForce = movementDirection * movementVariables.movementSpeed * Time.fixedDeltaTime;

            rb.AddForce(movementForce);
        }

    

        private Vector3 RotateInputToLocal(Vector3 movementInput)
        {
            return transform.rotation * movementInput;
        }

        public void SetDependencies(IInputGatherer inputGathererIn, IGroundedDetector groundDetectorIn)
        {
            inputGatherer = inputGathererIn;
            groundedDetector = groundDetectorIn;
        }

       

        [Serializable]
        private class MovementVariables
        {
            public float movementSpeed;
            public float maxVelocity;
            public float jumpForce;
          
        }
    }
}