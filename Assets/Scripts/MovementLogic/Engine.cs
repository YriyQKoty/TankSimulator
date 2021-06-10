using Interfaces;
using UnityEngine;

namespace MovementLogic
{
    public class Engine : IMoveable
    {
        /// <summary>
        /// Moves a rigidbody in a given direction
        /// </summary>
        /// <param name="rigidbody"></param>
        /// <param name="verticalDelta"></param>
        /// <param name="speed"></param>
        public void Move(Rigidbody rigidbody, float speed)
        {
            rigidbody.velocity = rigidbody.transform.right * speed;
        }
 
        /// <summary>
        /// Stops rigidbody
        /// </summary>
        /// <param name="rigidbody"></param>
        public void Stop(Rigidbody rigidbody)
        {
            return;
        }
    }
}
