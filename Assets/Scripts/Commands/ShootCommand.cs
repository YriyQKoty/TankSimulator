using Controllers;
using UnityEngine;

namespace Commands
{
    public class ShootCommand  :ICommand
    {
        private BulletSpawner _bulletSpawner;

        public ShootCommand(BulletSpawner spawner)
        {
            if (spawner == null)
            {
                Debug.LogError("Bullet spawner is null!");
                return;
            }

            _bulletSpawner = spawner;
        }
        public void Execute()
        {
            _bulletSpawner.Fire();
        }

        public bool CanExecute()
        {
            return Input.GetButtonDown("Fire1") && _bulletSpawner.CanShoot;
        }
    }
}