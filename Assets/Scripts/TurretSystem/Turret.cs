using Interfaces;
using UnityEngine;

namespace TurretSystem
{
    public class Turret : IRotatable
    {
        public void Rotate(Rigidbody rigidbody, Quaternion rotation)
        {
            rigidbody.transform.Rotate(rotation.x, rotation.y, rotation.z, Space.Self);
        }
    }
}