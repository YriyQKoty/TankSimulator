using Controllers;
using UnityEngine;

namespace Commands.RotationCommands
{
    public class TurretRotateCommand : ICommand
    {
        private TankController _tank;

        public TurretRotateCommand(TankController tank)
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
            //sends command to a tank to rotate turret on given euler angles
            _tank.RotateTurret(new Vector3(0,Input.GetAxis("Mouse X") * _tank.TurretRPM,0));

            if (!_tank.TurretSource.isPlaying)
            {
                SoundManager.Instance.PlayClip(_tank.TurretSource, SoundManager.Instance.TurretRotation);
            }
        }

        public bool CanExecute()
        {
            return Input.GetAxis("Mouse X") != 0;
        }
    }
}