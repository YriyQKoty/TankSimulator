using UnityEngine;

namespace Interfaces
{
    /// <summary>
    /// Describes moveable rigidbody entity
    /// </summary>
    public interface IMoveable
    {
        /// <summary>
        /// Moves a rigidbody
        /// </summary>
        /// <param name="rigidbody"></param>
        /// <param name="speed"></param>
        void Move(Rigidbody rigidbody, float speed);
        /// <summary>
        /// Stops rigidbody
        /// </summary>
        /// <param name="rigidbody"></param>
        void Stop(Rigidbody rigidbody);
    }
}