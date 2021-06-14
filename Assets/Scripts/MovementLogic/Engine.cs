using Interfaces;
using UnityEngine;

namespace MovementLogic
{
    public class Engine : IMoveable
    {
        /// <summary>
        /// Moves a rigidbody with given speed
        /// </summary>
        /// <param name="rigidbody"></param>
        /// <param name="speed"></param>
        public void Move(Rigidbody rigidbody, float speed)
        {
            //setting rigidbody velocity as a vector multiplied by speed
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
