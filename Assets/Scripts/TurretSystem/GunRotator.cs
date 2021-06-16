using System;
using UnityEngine;

namespace TurretSystem
{
    /// <summary>
    /// Rotates a gun
    /// </summary>
    public class GunRotator
    {
        public void Rotate(Transform rotator, float angle)
        {
            rotator.localEulerAngles = new Vector3(-angle, 0,0);
        }
    }
}