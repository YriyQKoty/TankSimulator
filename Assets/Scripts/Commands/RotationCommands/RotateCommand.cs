using Controllers;
using UnityEngine;

namespace Commands.RotationCommands
{
    /// <summary>
    /// Command for tank rotation
    /// </summary>
    public class RotateCommand : ICommand
    {
        /// <summary>
        /// Tank instance
        /// </summary>
        TankController _tank;
        public RotateCommand(TankController tank)
        {
            if (tank == null)
            {
                Debug.LogError("Tank is null!");
                return;
            }
            _tank = tank;
        }
   
        /// <summary>
        /// Executes rotation
        /// </summary>
        public void Execute()
        {
            var eulerAngVelocity = new Vector3(0, Input.GetAxis("Horizontal") * _tank.RotationVelocity, 0);
            _tank.TurnBody(eulerAngVelocity);
        }

        /// <summary>
        /// Checks if command can be executed
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {
            return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ||
                   Input.GetKey(KeyCode.LeftArrow);
        }
    }
}