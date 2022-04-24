using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Game.Camera;
using UnityEngine;
using Utility;
using Debug = System.Diagnostics.Debug;

namespace Player
{
    public class LookAligner : MonoBehaviour
    {
        private Transform cameraTransform;

        private void Awake()
        {
            // ReSharper disable once PossibleNullReferenceException
            cameraTransform = Camera.main.transform;
        }

        private void FixedUpdate()
        {
            var currentRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(currentRotation.x, cameraTransform.rotation.eulerAngles.y, currentRotation.z);
        }
    }
}