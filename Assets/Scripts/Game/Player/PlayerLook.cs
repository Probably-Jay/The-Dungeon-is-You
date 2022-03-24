using System;
using UnityEngine;
using Utility;

namespace Player
{
    public class PlayerLook : MonoBehaviour
    {
        [SerializeField] private LookVariables lookVariables;
        private ILookGatherer lookGatherer;
        

        private void Awake()
        {
            this.AssignGetComponentTo<LookGatherer, ILookGatherer>(out lookGatherer);
        }

        private void FixedUpdate()
        {
            ApplyHorizontalRotation();
            ApplyVerticalRotation();
        }

        private void ApplyVerticalRotation()
        {
            //throw new NotImplementedException();
        }

        private void ApplyHorizontalRotation()
        {
            var rotationDelta = lookGatherer.ReadMouseXDelta();
            var scaledRotationDelta = rotationDelta * lookVariables.xRotationSpeed * Time.fixedDeltaTime;

            var currentRotation = transform.rotation.eulerAngles; 
            var newYRotation = currentRotation.y + scaledRotationDelta; // y is the horizontal axis

            transform.localRotation = Quaternion.Euler(currentRotation.x, newYRotation, currentRotation.z );
        }
        
        [Serializable]
        private class LookVariables
        {
            public float xRotationSpeed;
            public float yRotationSpeed;
        }
    }
}