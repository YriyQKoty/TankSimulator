using UnityEngine;

namespace Commands.RotationCommands
{
    public class TurretRotation : ICommand
    {
        private TankUnit _tank;

        public TurretRotation(TankUnit tank)
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
            _tank.RotateTurret(0,Input.GetAxis("Mouse X"),0);
        }

        public bool CanExecute()
        {
            return true;
        }
    }
}