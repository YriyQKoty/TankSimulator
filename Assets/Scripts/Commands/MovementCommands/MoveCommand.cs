using Controllers;
using UnityEngine;

namespace Commands.MovementCommands
{
    /// <summary>
    /// Command which moves tank
    /// </summary>
    public class MoveCommand : ICommand
    {
        /// <summary>
        /// Tank instance
        /// </summary>
        TankController _tank;

        /// <summary>
        /// Direction
        /// </summary>
        private Direction _direction;

        private float _currentVelocity;

        /// <summary>
        /// Creates a command for a tank with direction
        /// </summary>
        /// <param name="tank"></param>
        /// <param name="direction"></param>
        public MoveCommand(TankController tank, Direction direction)
        {
            if (tank == null)
            {
                Debug.LogError("Tank is null!");
                return;
            }

            _tank = tank;
            _direction = direction;
            //assign current velocity depending on a direction
            _currentVelocity = direction == Direction.Forward ? _tank.FrontalVelocity : _tank.BackwardVelocity;
        }

        /// <summary>
        /// Executes command
        /// </summary>
        public void Execute()
        {
            var verticalDelta = Input.GetAxis("Vertical");
            _tank.Move(verticalDelta, _currentVelocity);
        }

        /// <summary>
        /// Check if command can be executed
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {
            //if moving forward - check for right input 
            if (_direction == Direction.Forward)
            {
                return (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
            }

            //if moving backward - check for right input 
            if (_direction == Direction.Backward)
            {
                return (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow));
            }

            return false;
        }
    }

    /// <summary>
    /// Types of movement
    /// </summary>
    public enum Direction
    {
        Forward,
        Backward
    }
}