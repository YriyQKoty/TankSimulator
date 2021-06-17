using System;
using System.Collections.Generic;
using Interfaces;
using TrackSystem;
using UnityEngine;

namespace MovementLogic
{
    public class TrackSystemController : MonoBehaviour, IMoveable
    {
        
        private Wheel[] leftTrackWheels;
        private Wheel[] rightTrackWheels;
        
        [Header("Tracks")]
        [SerializeField] private GameObject leftTrack;
        [SerializeField] private GameObject rightTrack;
        
        [Space]
        [Header("Left track")]
        public Transform[] leftUpperWheelsTransforms;
        public Transform[] leftTrackWheelsTransforms;
        public Transform[] leftTrackBones;

        [Space]
        [Header("Right track")]
        public Transform[] rightUpperWheelsTransforms;
        public Transform[] rightTrackWheelsTransforms;
        public Transform[] rightTrackBones;
        
        
        private void Awake()
        {
            leftTrackWheels = new Wheel[leftTrackWheelsTransforms.Length];
            rightTrackWheels = new Wheel[rightTrackWheelsTransforms.Length];

            //initialising left and right wheels
            for (int i = 0; i < leftTrackWheelsTransforms.Length; i++)
            {
                leftTrackWheels[i] = new Wheel(leftTrackWheelsTransforms[i], leftTrackBones[i]);
            }

            for (int i = 0; i < rightTrackWheelsTransforms.Length; i++)
            {
                rightTrackWheels[i] = new Wheel(rightTrackWheelsTransforms[i], rightTrackBones[i]);
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
        
        public float rotateOnStandTorque = 1500.0f; //1
        public float rotateOnStandBrakeTorque = 500.0f; //2
        public float maxBrakeTorque = 1000.0f; //3
        public float forwardTorque = 500.0f; //1
        public float rotateOnMoveBrakeTorque = 400.0f; //2 
        public float minBrakeTorque = 0.0f; //3 
        public float minOnStayStiffness = 0.06f; //4 
        public float minOnMoveStiffness = 0.05f;  //5 
        public float rotateOnMoveMultiply = 2.0f; //6
        
        public void CalculateMotorForce(WheelCollider col, float accel, float steer = 0){  //6
            
            WheelFrictionCurve fc = col.sidewaysFriction;  //7 
            
            if(accel == 0 && steer == 0){ //7
                col.brakeTorque = maxBrakeTorque; //7
            }else if(accel == 0.0f){  //8
                col.brakeTorque = rotateOnStandBrakeTorque; //9
                col.motorTorque = steer*rotateOnStandTorque; //10
            } 
            else{ //8 
		 
                col.brakeTorque = minBrakeTorque;  //9 
                col.motorTorque = accel*forwardTorque;  //10 
					 
                if(steer < 0){ //11 
                    col.brakeTorque = rotateOnMoveBrakeTorque; //12 
                    col.motorTorque = steer*forwardTorque*rotateOnMoveMultiply;//13 
                    fc.stiffness = 1.0f + minOnMoveStiffness - Mathf.Abs(steer);  //14 
                } 
		 
                if(steer > 0){ //15 
			 
                    col.motorTorque = steer*forwardTorque*rotateOnMoveMultiply;//16 
                    fc.stiffness = 1.0f + minOnMoveStiffness - Mathf.Abs(steer); //17
                } 
		 
					 
            } 
            
             
            if(fc.stiffness > 1.0f)fc.stiffness = 1.0f; //18		 
            col.sidewaysFriction = fc; //19
	 
            if(col.rpm > 0 && accel < 0){ //20 
                col.brakeTorque = maxBrakeTorque;  //21
            }else if(col.rpm < 0 && accel > 0){ //22 
                col.brakeTorque = maxBrakeTorque; //23
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
        /// Moves track with given speed
        /// </summary>
        /// <param name="speed"></param>
        public void Move(float speed)
        {
            var rpm = CalculateAverageRpm(leftTrackWheels);

            //rotating left wheels
            foreach (var wheel in leftTrackWheels)
            {
                wheel.Update(transform, rpm);
                
                CalculateMotorForce(wheel.Collider, speed);
            }

            //updating textures

            rpm = CalculateAverageRpm(rightTrackWheels);

            //rotating right wheels
            foreach (var wheel in rightTrackWheels)
            {
                wheel.Update(transform, rpm);
                CalculateMotorForce(wheel.Collider, speed);
            }

            //updating textures
            
            RotateUpperWheels();

         
        }

        //setting rigidbody velocity as a vector multiplied by speed
        //rigidbody.velocity = rigidbody.transform.forward * speed;

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