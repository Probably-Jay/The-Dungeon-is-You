using System;
using UnityEngine;
using Utility;

namespace Player
{
    public interface IGroundedDetector
    {
        bool IsGrounded { get; }
    }

    public class GroundedDetector : MonoBehaviour, IGroundedDetector
    {
        private SphereCollider feet;

        [SerializeField] private SphereCastVariables sphereCastVariables;
        [SerializeField] private bool debug = false;
        [SerializeField] bool grounded;

        private void Awake()
        {
            this.AssignGetComponentTo(out feet);
        }

        public bool IsGrounded => grounded;

        private void FixedUpdate()
        {
            GroundDetection();
        }

        private void GroundDetection()
        {
            var origin = feet.transform.TransformPoint(feet.center + Vector3.up * 0.5f);
            var radius = feet.radius;
            var direction = Vector3.down;
            var distance = sphereCastVariables.distance;
            
            var hit = Physics.SphereCast(
                origin: origin,
                radius: radius, 
                direction: direction,
                hitInfo: out var hitInfo,
                maxDistance: distance,
                layerMask: ~LayerMask.NameToLayer("Default"),
                queryTriggerInteraction: QueryTriggerInteraction.Ignore
            );

            grounded = hit;
        }

        private void OnDrawGizmos()
        {
            if(!debug || feet == null) return;
            
            var origin = feet.transform.TransformPoint(feet.center + Vector3.up * 0.5f);
            var radius = feet.radius;
            var direction = Vector3.down;
            var distance = sphereCastVariables.distance;
                
            Gizmos.color = new Color(0,0,1,0.2f);
            Gizmos.DrawSphere(origin,radius);
            Gizmos.color = new Color(0,1,1,0.2f);
            Gizmos.DrawSphere(origin+direction*distance,radius);
        }

        [Serializable]
        struct SphereCastVariables
        {
            public float distance;
        }
        
    }
}