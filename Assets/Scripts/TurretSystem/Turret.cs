using Interfaces;
using UnityEngine;

namespace TurretSystem
{
    public class Turret : IRotatable
    {
        private GunRotator _rotator;
        private Transform _rotatorTransform;
        public Turret(Transform gunRotTransform)
        {
            if (gunRotTransform == null)
            {
                Debug.LogError("GunRotator transform is null!");
                return;
            }
            
            _rotator = new GunRotator();
            _rotatorTransform = gunRotTransform;
        }
        public void Rotate(Rigidbody rigidbody, Quaternion rotation)
        {
            rigidbody.transform.Rotate(rotation.x, rotation.y, rotation.z, Space.Self);
        }

        public void RotateGun(float zAngle)
        {
            _rotator.Rotate(_rotatorTransform,zAngle);
        }
    }
}