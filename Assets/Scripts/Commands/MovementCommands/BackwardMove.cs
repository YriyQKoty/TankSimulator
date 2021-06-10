using UnityEngine;

namespace Commands.MovementCommands
{
    /// <summary>
    /// Command for backward movement
    /// </summary>
    public class BackwardMove : ICommand
    {
        /// <summary>
        /// Tank instance
        /// </summary>
        TankUnit _tank;

        public BackwardMove(TankUnit tank)
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
            _tank.Move(verticalDelta, _tank.BackwardVelocity);
        }

        /// <summary>
        /// Check if command can be executed
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {
            return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        }
        
    }
}