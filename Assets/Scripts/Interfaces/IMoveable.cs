using UnityEngine;

namespace Interfaces
{
    /// <summary>
    /// Describes moveable rigidbody entity
    /// </summary>
    public interface IMoveable
    {
        /// <summary>
        /// Moves gameobject
        /// </summary>
        /// <param name="speed"></param>
        void Move(float speed); 
        /// <summary>
        /// Stops gameobject
        /// </summary>
        void Stop();
    }
}