using Interfaces;
using UnityEngine;

namespace MovementLogic
{
    /// <summary>
    /// Steering system of a tank
    /// </summary>
    public class SteeringSystem : IRotatable
    {
        /// <summary>
        /// Turns a rigidbody on a delta rotation
        /// </summary>
        /// <param name="rigidbody">gameobject with a rigidbody</param>
        /// <param name="deltaRotation">delta for rotation</param>
        public void Rotate(Rigidbody rigidbody, Quaternion deltaRotation)
        {
           rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
        }
    }
}