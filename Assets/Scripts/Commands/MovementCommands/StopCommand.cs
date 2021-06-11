using Controllers;
using UnityEngine;

namespace Commands.MovementCommands
{
    /// <summary>
    /// Command which stops a tank
    /// </summary>
    public class StopCommand : ICommand
    {
        TankController _tank;

        public StopCommand(TankController tank)
        {
            if (tank == null)
            {
                Debug.LogError("Tank is null!");
                return;
            }
            _tank = tank;
        }

        public void Execute()
        {
            _tank.Stop();
        }

        public bool CanExecute()
        {
            return Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S);
        }
    }
}