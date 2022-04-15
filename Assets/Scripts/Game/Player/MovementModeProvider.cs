using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

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
            Mode = MovementMode.Running;
        } 

        public IMovement Current => modes[Mode];
        public MovementMode Mode { get; set; }
    }
}