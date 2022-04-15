using System;
using UnityEngine;
using Utility;

namespace Player
{
    public class PlayerLook : MonoBehaviour
    {
        private ILookGatherer lookGatherer;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private LookVariables lookVariables;

        private void Awake()
        {
            this.AssignGetComponentTo<LookGatherer, ILookGatherer>(out lookGatherer);
            this.AssertNotNull(cameraTransform);
        }

        private void Update()
        {
            ApplyHorizontalRotation();
            ApplyVerticalRotation();
        }

        private void ApplyHorizontalRotation()
        {
            var rotationDelta = lookGatherer.ReadMouseHorizontalDelta();
            var scaledRotationDelta = rotationDelta * lookVariables.rotationSpeed.x; //* Time.deltaTime; ?

            var currentRotation = transform.rotation.eulerAngles; 
            var newYRotation = currentRotation.y + scaledRotationDelta; // y is the vertical axis which will apply horizontal rotation

            
            transform.rotation = Quaternion.Euler(currentRotation.x, newYRotation, currentRotation.z );
        }

        private void ApplyVerticalRotation()
        {
            var rotationDelta = lookGatherer.ReadMouseVerticalDelta();
            var scaledRotationDelta = rotationDelta * lookVariables.rotationSpeed.y; //* Time.deltaTime; ?
            
            var currentLookRotation = cameraTransform.rotation.eulerAngles; 
            
            var newXRotation = currentLookRotation.x + scaledRotationDelta; // x is the horizontal axis which will apply vertical rotation
            
            newXRotation = ClampToView(newXRotation);
            
            cameraTransform.rotation = Quaternion.Euler(newXRotation, currentLookRotation.y, currentLookRotation.z ); // i have no idea why this needs to be rotation not localrotation
        }

        private float ClampToView(float newXRotation)
        {
            var normalisedXRotation = newXRotation > 180 ? newXRotation - 360f : newXRotation;
            return Mathf.Clamp(normalisedXRotation, -lookVariables.maxCamAngle, lookVariables.maxCamAngle);
        }

        [Serializable]
        private class LookVariables
        {
            public Vector2 rotationSpeed;
            public float maxCamAngle;
        }
    }
}