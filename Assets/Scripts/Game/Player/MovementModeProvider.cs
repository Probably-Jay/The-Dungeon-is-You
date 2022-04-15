using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Player
{
    public interface IMovement
    {
        void ApplyMovement(Vector3 movementDirection);
        bool ReachedMaxMovementVelocity { get; }
    }


    public class MovementModeProvider : MonoBehaviour
    {
        [SerializeField] private MovementMode debugForcedMode = MovementMode.None;
        [SerializeField,Description("Debug readout of current mode (readonly)")] private MovementMode debugCurrentModeReadonly = MovementMode.None;
        public enum MovementMode
        {
            None,
            Walking,
            Running
        }
        [SerializeField] private Walking walking;
        [SerializeField] private Running running;

        private ReadOnlyDictionary<MovementMode, IMovement> modes;

        public void AssignDependencies(Rigidbody rb, PlayerMovement.MovementVariables variables)
        {
            modes = new ReadOnlyDictionary<MovementMode, IMovement>(new  Dictionary<MovementMode, IMovement>()
            {
                {MovementMode.Walking, new Walking(rb,variables)},
                {MovementMode.Running, new Running(rb,variables)}
                
            });
            Mode = MovementMode.Walking;
        } 

        public IMovement Current => modes[Mode];
        public MovementMode Mode { get; private set; }

        public void UpdateMovementMode(bool sprintButtonPressed, Vector3 rbVelocity,
            PlayerMovement.MovementVariables movementVariables)
        {
#if UNITY_EDITOR
            if (debugForcedMode != MovementMode.None)
            {
                Mode = debugForcedMode;
                return;
            }
#endif
            Mode = ModeDeterminer.DetermineMovementMode(Mode, sprintButtonPressed, rbVelocity,movementVariables.minRunVelocity);
            debugCurrentModeReadonly = Mode;
        }
       
    }
    internal static class ModeDeterminer
    {
        public static MovementModeProvider.MovementMode DetermineMovementMode(
            MovementModeProvider.MovementMode currentMode, bool sprintButtonWasPressed, Vector3 rbVelocity,
            float minRunVelocity)
        {
            if (NotMovingFastEnough(rbVelocity, minRunVelocity))
                return MovementModeProvider.MovementMode.Walking;

            if (ModeIsWalking(currentMode))
                return sprintButtonWasPressed switch
                {
                    true => MovementModeProvider.MovementMode.Running,
                    false => MovementModeProvider.MovementMode.Walking
                };
            
            return currentMode;
        }

        private static bool ModeIsWalking(MovementModeProvider.MovementMode currentMode) 
            => currentMode == MovementModeProvider.MovementMode.Walking;

        private static bool NotMovingFastEnough(Vector3 rbVelocity, float minRunVelocity) 
            => rbVelocity.magnitude <= minRunVelocity;
    }
}
