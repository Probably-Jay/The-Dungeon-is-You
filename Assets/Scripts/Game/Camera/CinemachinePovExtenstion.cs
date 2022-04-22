using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Player;


namespace Game.Camera.CinemachineExtenstions
{
    public class CinemachinePovExtenstion : CinemachineExtension
    {
        private LookGatherer lookGatherer;
        private Vector3 startingRotation;

        protected override void Awake()
        {
            startingRotation = transform.localRotation.eulerAngles;
            base.Awake();
        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (vcam.Follow == null) return;
            if (stage == CinemachineCore.Stage.Aim)
            {
             //   Vector2 deltaInput = inputManager.GetPlayerLookMouseDelta() * Time.deltaTime;
            }

        }
    }
}
