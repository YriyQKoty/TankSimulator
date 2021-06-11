using System;
using Controllers;
using TurretSystem;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Commands.RotationCommands
{
    public class GunRotateCommand : ICommand
    {
        private Turret _turret;
        private TankController _tankController;
        private float _rotationAngle;

        public GunRotateCommand(TankController tankController)
        {
            if (tankController == null)
            {
                Debug.LogError("TankUnit is null!");
                return;
            }

            _tankController = tankController;
            _turret = _tankController.Turret;
        }
        public void Execute()
        {
            _rotationAngle += Input.GetAxis("Mouse Y");
            _rotationAngle = Mathf.Clamp(_rotationAngle, _tankController.MinRotationAngle,
                _tankController.MaxRotationAngle);
            _turret.RotateGun(_rotationAngle);
        }

        public bool CanExecute()
        {
            return true;
        }
    }
}