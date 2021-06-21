using MovementLogic;
using TurretSystem;
using UnityEngine;

namespace Controllers
{
    public class TankController : MonoBehaviour
    {
        #region DataFields
        
        /// <summary>
        /// Track system controller component
        /// </summary>
        [SerializeField] private TrackSystemController _trackSystemController;

        /// <summary>
        /// Steering component
        /// </summary>
        private SteeringSystem _steering = new SteeringSystem();

        /// <summary>
        /// Turret component
        /// </summary>
        private Turret _turret;

        /// <summary>
        /// Tank RigidBody
        /// </summary>
        private Rigidbody _rigidbody;
        
        [SerializeField] private Transform centerOfMass;
        
       
        /// <summary>
        /// Frontal speed
        /// </summary>
        [Header("Engine speeds")] [Range(20, 60)] [SerializeField]
        private int frontalVelocity = 30;

  
        /// <summary>
        /// Backward speed
        /// </summary>
        [Range(10, 25)] [SerializeField] private int backwardVelocity = 15;
        
        /// <summary>
        /// Curve for torque value (moving backward)
        /// </summary>
        [Header("Torque curves")][SerializeField] AnimationCurve backwardVelTorque = AnimationCurve.Linear(0, 50, 50, 0);
        /// <summary>
        /// Curve for torque value (moving forward)
        /// </summary>
        [SerializeField] AnimationCurve frontalVelTorque = AnimationCurve.Linear(0, 200, 50, 0);



        /// <summary>
        /// Rotation speed
        /// </summary>
        [Space] [Header("Body turn speed")] [Range(10, 30)] [SerializeField]
        private int rotationVelocity = 10;


        [Space] [Header("Turret rotation per minute")] [Range(0, 2)] [SerializeField]
        private float turretRpm = 0.5f;

        [SerializeField] private Rigidbody rotationBody;

        [Header("Gun rotator")] [SerializeField]
        private Transform gunRotator;

        [Header("Angles")] [Range(-5, 0)] [SerializeField]
        private float minRotationAngle;

        [Range(5, 25)] [SerializeField] private float maxRotationAngle;

       [SerializeField] private Camera _camera;

        public Camera Camera => _camera;

        private float currentVelocity;


        [Header("Bullets")] [SerializeField] private BulletSpawner _bulletSpawner;

        //Sound sources
        [Header("Sound sources")]
        [SerializeField]private AudioSource engineSource;
        [SerializeField]private AudioSource turretSource;
        [SerializeField]private AudioSource trackSource;
        [SerializeField] private AudioSource shootSource;
        
        #endregion
        
        #region Properties

        /// <summary>
        /// Accessor for frontal speed
        /// </summary>
        public int FrontalVelocity => frontalVelocity;
        public AnimationCurve FrontalVelTorque => frontalVelTorque;

        /// <summary>
        /// Accessor for backward speed
        /// </summary>
        public int BackwardVelocity => backwardVelocity;
        public AnimationCurve BackwardVelTorque => backwardVelTorque;


        /// <summary>
        /// Accessor for rotation speed
        /// </summary>
        public int RotationVelocity => rotationVelocity;


        public float TurretRPM => turretRpm;
        /// <summary>
        /// Accessor for min rot angle
        /// </summary>
        public float MinRotationAngle => minRotationAngle;

        /// <summary>
        /// Accessor for max rot angle
        /// </summary>
        public float MaxRotationAngle => maxRotationAngle;

        /// <summary>
        /// Accessor for turret
        /// </summary>
        public Turret Turret => _turret;

        /// <summary>
        /// Accessor for gun rotator
        /// </summary>
        public Transform GunRotator => gunRotator;

        public AudioSource EngineSource => engineSource;

        public AudioSource TurretSource => turretSource;

        public AudioSource TrackSource => trackSource;

        public AudioSource ShootSource => shootSource;

        public BulletSpawner BulletSpawner => _bulletSpawner;

        #endregion

        #region BuiltInMethods

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.centerOfMass = centerOfMass.localPosition;
            //creating turret component with a gun rotator
            _turret = new Turret(gunRotator);
        }

        #endregion

        #region CustomMethods

        /// <summary>
        ///  Moves a tank with corresponding velocity
        /// </summary>
        /// <param name="verticalDelta">value of "Vertical" axis for WS<!--<>--></param>
        /// <param name="velocity">speed</param>
        public void Move(float verticalDelta, float velocity)
        {
            //calculates current tank speed
            currentVelocity = 2 * Mathf.PI * _trackSystemController.RPM * 60 / 1000f;
            if (Mathf.Abs(currentVelocity) > velocity)
            {
                _trackSystemController.OnSpeedLimit();
            }
            
            _trackSystemController.Move(verticalDelta * velocity);
           
        }

        /// <summary>
        /// Rotates tank body on a given euler rotation vector
        /// </summary>
        /// <param name="rotationAroundY"></param>
        public void TurnBody(Vector3 rotationAroundY)
        {
            //defining delta for rotating a rigidbody as a Quaterinion of rotation around Y axis multiplied by fixedDeltaTime
            var deltaRotation = Quaternion.Euler(rotationAroundY * Time.fixedDeltaTime);
            _steering.Rotate(_rigidbody, deltaRotation);
        }

        /// <summary>
        /// Stops tank
        /// </summary>
        public void Stop()
        {
            _trackSystemController.Stop();
        }

        /// <summary>
        /// Rotates turret with given angles
        /// </summary>
        /// <param name="eulerAngles"></param>
        public void RotateTurret(Vector3 eulerAngles)
        {
            _turret.Rotate(rotationBody,
                new Quaternion(eulerAngles.x, eulerAngles.y * turretRpm * Time.deltaTime, eulerAngles.z, 1));
        }

        #endregion
    }
}