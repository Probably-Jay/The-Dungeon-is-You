using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Player
{
    public interface IMovement
    {
        void ApplyMovement(Vector3 movementDirection);
        bool ReachedMaxMovementVelocity { get; }
    }


    internal class MovementModeProvider
    {
        public enum MovementMode
        {
            Walking,
            Running
        }
        private Running running;
        private Walking walking;

        private readonly ReadOnlyDictionary<MovementMode, IMovement> modes;

        public MovementModeProvider(Rigidbody rb, PlayerMovement.MovementVariables variables)
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

        public void UpdateMovementMode(bool sprintButtonPressed, Vector3 rbVelocity)
        {
            Mode = ModeDeterminer.DetermineMovementMode(Mode, sprintButtonPressed, rbVelocity);
        }
       
    }
    internal static class ModeDeterminer
    {
        public static MovementModeProvider.MovementMode DetermineMovementMode(MovementModeProvider.MovementMode currentMode, bool sprintButtonWasPressed, Vector3 rbVelocity)
        {
            if (NotMoving(rbVelocity))
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

        private static bool NotMoving(Vector3 rbVelocity) 
            => rbVelocity == Vector3.zero;
    }
}
