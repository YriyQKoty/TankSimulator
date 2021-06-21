using System;
using System.Collections.Generic;
using Controllers;
using Interfaces;
using TrackSystem;
using UnityEngine;

namespace MovementLogic
{
    public class TrackSystemController : MonoBehaviour, IMoveable
    {
        private TankController _tankController;
        private Wheel[] leftTrackWheels;
        private Wheel[] rightTrackWheels;

        [Header("Tracks")] [SerializeField] private GameObject leftTrack;
        [SerializeField] private GameObject rightTrack;

        [Space] [Header("Left track")] public Transform[] leftUpperWheelsTransforms;
        public Transform[] leftTrackWheelsTransforms;
        public Transform[] leftTrackBones;

        [Space] [Header("Right track")] public Transform[] rightUpperWheelsTransforms;
        public Transform[] rightTrackWheelsTransforms;
        public Transform[] rightTrackBones;
        
        
        //force moments
        public float rotateOnStandBrakeTorque = 500.0f;
        public float maxBrakeTorque = 1000.0f;
        public float minBrakeTorque = 0.0f;


        /// <summary>
        /// Rotates per minute (for wheels)
        /// </summary>
        public float RPM
        {
            get => CalculateAverageRpm(leftTrackWheels);
            private set { }
        }


        private void Awake()
        {
            _tankController = GetComponent<TankController>();
            leftTrackWheels = new Wheel[leftTrackWheelsTransforms.Length];
            rightTrackWheels = new Wheel[rightTrackWheelsTransforms.Length];

            //initialising left and right wheels
            for (int i = 0; i < leftTrackWheelsTransforms.Length; i++)
            {
                leftTrackWheels[i] = new Wheel(leftTrackWheelsTransforms[i], leftTrackBones[i],
                    leftTrackWheelsTransforms[i].GetComponentInParent<WheelCollider>());
            }

            for (int i = 0; i < rightTrackWheelsTransforms.Length; i++)
            {
                rightTrackWheels[i] = new Wheel(rightTrackWheelsTransforms[i], rightTrackBones[i],
                    rightTrackWheelsTransforms[i].GetComponentInParent<WheelCollider>());
            }
        }

        /// <summary>
        /// Calculates average rotation per minute
        /// </summary>
        /// <param name="wheels"></param>
        /// <returns></returns>
        float CalculateAverageRpm(Wheel[] wheels)
        {
            float rpm = 0.0f;

            //list of indexes for grounded wheels
            List<int> groundedWheelsIndexes = new List<int>();

            for (int i = 0; i < wheels.Length; i++)
            {
                if (wheels[i].Collider.isGrounded)
                {
                    groundedWheelsIndexes.Add(i);
                }
            }

            if (groundedWheelsIndexes.Count == 0)
            {
                foreach (Wheel wd in wheels)
                {
                    rpm += wd.Collider.rpm;
                }

                rpm /= wheels.Length;
            }
            else
            {
                for (int i = 0; i < groundedWheelsIndexes.Count; i++)
                {
                    rpm += wheels[groundedWheelsIndexes[i]].Collider.rpm;
                }

                rpm /= groundedWheelsIndexes.Count;
            }

            return rpm;
        }

        /// <summary>
        /// Should be called on speed limit
        /// </summary>
        public void OnSpeedLimit()
        {
            foreach (var wheel in leftTrackWheels)
            {
                //motor moment = 0
                wheel.Collider.motorTorque = 0;
            }

            foreach (var wheel in rightTrackWheels)
            {
                wheel.Collider.motorTorque = 0;
            }
        }

        /// <summary>
        /// Calculates motor moment
        /// </summary>
        /// <param name="col">Wheel collider</param>
        /// <param name="accel">Acceleration (velocity*vertical axis delta)</param>
        public void CalculateMotorForce(WheelCollider col, float accel)
        {
            WheelFrictionCurve fc = col.sidewaysFriction;


            var wheelVelocity = 2 * Mathf.PI * col.radius * RPM * 60 / 1000f; //how fast tank is moving
            //moving forward
            if (accel > 0 && col.rpm >= 0)
            {
                //motor moment
                col.motorTorque = accel * _tankController.FrontalVelTorque.Evaluate(wheelVelocity);
                col.brakeTorque = minBrakeTorque; //brake moment
            } //moving backward
            else if (accel < 0 && col.rpm <= 0)
            {
                //motor moment
                col.motorTorque = accel * _tankController.BackwardVelTorque.Evaluate(Mathf.Abs(wheelVelocity));
                col.brakeTorque = minBrakeTorque;
            }
            else //stopping
            {
                col.brakeTorque = rotateOnStandBrakeTorque;
            }


            if (fc.stiffness > 1.0f) fc.stiffness = 1.0f;
            col.sidewaysFriction = fc;

            //changing direction - brake moment
            if (col.rpm > 0 && accel < 0)
            {
                col.brakeTorque = maxBrakeTorque;
            }
            else if (col.rpm < 0 && accel > 0)
            {
                col.brakeTorque = maxBrakeTorque;
            }
        }

        /// <summary>
        /// Rotates upper (useless wheels)
        /// </summary>
        void RotateUpperWheels()
        {
            for (int i = 0; i < leftUpperWheelsTransforms.Length; i++)
            {
                leftUpperWheelsTransforms[i].localRotation = Quaternion.Euler(leftTrackWheels[0].RotationAngle,
                    leftTrackWheels[0].StartWheelAngle.y, leftTrackWheels[0].StartWheelAngle.z);
            }

            for (int i = 0; i < rightUpperWheelsTransforms.Length; i++)
            {
                rightUpperWheelsTransforms[i].localRotation = Quaternion.Euler(rightTrackWheels[0].RotationAngle,
                    rightTrackWheels[0].StartWheelAngle.y, rightTrackWheels[0].StartWheelAngle.z);
            }
        }

        /// <summary>
        /// Moves tank with given speed
        /// </summary>
        /// <param name="speed"></param>
        public void Move(float speed)
        {
            RPM = CalculateAverageRpm(leftTrackWheels);

            //rotating left wheels
            foreach (var wheel in leftTrackWheels)
            {
                wheel.Update(transform, RPM);

                CalculateMotorForce(wheel.Collider, speed);
            }

            //updating textures

            RPM = CalculateAverageRpm(rightTrackWheels);

            //rotating right wheels
            foreach (var wheel in rightTrackWheels)
            {
                wheel.Update(transform, RPM);
                CalculateMotorForce(wheel.Collider, speed);
            }

            //updating textures

            RotateUpperWheels();
        }
        
        /// <summary>
        /// Stops rigidbody
        /// </summary>
        public void Stop()
        {
            foreach (var wheel in leftTrackWheels)
            {
                wheel.Collider.brakeTorque = Mathf.Infinity;
            }

            foreach (var wheel in rightTrackWheels)
            {
                wheel.Collider.brakeTorque = Mathf.Infinity;
            }
        }
    }
}