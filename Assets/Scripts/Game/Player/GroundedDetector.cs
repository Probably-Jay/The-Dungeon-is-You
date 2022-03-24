using UnityEngine;

namespace Player
{
    public interface IGroundedDetector
    {
        bool CanWalk { get; }
    }

    class GroundedDetector : MonoBehaviour, IGroundedDetector
    {
        public bool CanWalk => true;
    }
}