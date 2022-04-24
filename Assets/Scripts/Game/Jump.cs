using System;
using System.Collections;
using System.Collections.Generic;
using CustomDebug;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Utility;

namespace Player
{
    public class Jump : MonoBehaviour
    {
        private IGroundedDetector groundDetector;
        private IInputGatherer inputGatherer;

        [SerializeField] private JumpVariables jumpVariables;
        private Rigidbody rb;

        private bool CanJump => groundDetector.IsGrounded;

        private void Awake()
        {
            this.AssignGetComponentTo<GroundedDetector, IGroundedDetector>(out groundDetector);
            this.AssignGetComponentTo<InputGatherer, IInputGatherer>(out inputGatherer);
            
            this.AssignGetComponentTo(out rb);
        }
        
        private void FixedUpdate()
        {
            JumpProcedure();
        }

        private void JumpProcedure()
        {
            if(!CanJump) return;
            
            var jumpInput = inputGatherer.ReadShouldJump();
            if(!jumpInput) return;

            ApplyJump();
        }

        private void ApplyJump()
        {
            var jumpForce = jumpVariables.jumpForce * Vector3.up;
            rb.AddForce(jumpForce, ForceMode.Impulse);
        }

        [Serializable]
        private class JumpVariables
        {
            public float jumpForce;
        }
        
    }
}