using UnityEngine;

namespace Commands.MovementCommands
{
    /// <summary>
    /// Command which moves tank forward
    /// </summary>
    public class ForwardMove : ICommand
    {
        /// <summary>
        /// Tank instance
        /// </summary>
        TankUnit _tank;

        public ForwardMove(TankUnit tank)
        {
            if (tank == null)
            {
                Debug.LogError("Tank is null!");
                return;
            }
            
            _tank = tank;
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
            return (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
        }
    }
}