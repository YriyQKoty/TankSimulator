using System;
using UnityEngine;

namespace TurretSystem
{
    /// <summary>
    /// Rotates a gun
    /// </summary>
    public class GunRotator : MonoBehaviour
    {
        [Header("Angles")]
        [Range(-10, 5)] [SerializeField] private float minRotationAngle;
        [Range(15, 25)] [SerializeField] private float maxRotationAngle;

        private Transform _transform;

        private void Start()
        {
            ///
            _transform = transform;
        }
    }
}