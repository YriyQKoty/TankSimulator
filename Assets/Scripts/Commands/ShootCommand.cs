using Controllers;
using UnityEngine;

namespace Commands
{
    public class ShootCommand  :ICommand
    {
        private BulletSpawner _bulletSpawner;
        private TankController _tankController;

        public ShootCommand(TankController tank)
        {
            if (tank == null)
            {
                Debug.LogError("Bullet spawner is null!");
                return;
            }

            _tankController = tank;
            _bulletSpawner = _tankController.BulletSpawner;
        }
        public void Execute()
        {
            _bulletSpawner.Fire();

            if (!_tankController.ShootSource.isPlaying)
            {
                SoundManager.Instance.PlayClip(_tankController.ShootSource,SoundManager.Instance.Shoot);
            }
            
        }

        public bool CanExecute()
        {
            return Input.GetButtonDown("Fire1") && _bulletSpawner.CanShoot;
        }
    }
}