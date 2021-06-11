using System;
using UnityEngine;

namespace TurretSystem
{
    /// <summary>
    /// Rotates a gun
    /// </summary>
    public class GunRotator
    {
        public void Rotate(Transform rotator, float zAngle)
        {
            rotator.localEulerAngles = new Vector3(0,0,zAngle);
        }
    }
}