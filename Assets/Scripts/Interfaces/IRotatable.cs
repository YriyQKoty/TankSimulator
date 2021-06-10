using UnityEngine;


namespace Interfaces
{
    /// <summary>
    /// Describes rotatable entity
    /// </summary>
    public interface IRotatable
    {
        /// <summary>
        /// Rotates rigidbody
        /// </summary>
        /// <param name="rigidbody"></param>
        /// <param name="rotation"></param>
        void Rotate(Rigidbody rigidbody, Quaternion rotation);
    }
}