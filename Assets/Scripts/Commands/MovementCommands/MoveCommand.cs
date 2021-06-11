using Controllers;
using UnityEngine;

namespace Commands.MovementCommands
{
    /// <summary>
    /// Command which moves tank forward
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

        public MoveCommand(TankController tank, Direction direction)
        {
            if (tank == null)
            {
                Debug.LogError("Tank is null!");
                return;
            }

            _tank = tank;
            _direction = direction;
            _currentVelocity = direction == Direction.Forward ? _tank.FrontalVelocity : _tank.BackwardVelocity;
        }

        /// <summary>
        /// Executes command
        /// </summary>
        public void Execute()
        {
            var verticalDelta = Input.GetAxis("Vertical");
            _tank.Move(verticalDelta, _tank.FrontalVelocity);
        }

        /// <summary>
        /// Check if command can be executed
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {
            if (_direction == Direction.Forward)
            {
                return (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
            }

            if (_direction == Direction.Backward)
            {
                return (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow));
            }

            return false;
        }
    }

    public enum Direction
    {
        Forward,
        Backward
    }
}