using MovementLogic;
using TurretSystem;
using UnityEngine;

namespace Controllers
{
    public class TankController : MonoBehaviour
    {
        #region DataFields

        /// <summary>
        /// Engine component
        /// </summary>
        private Engine _engine = new Engine();

        /// <summary>
        /// Steering component
        /// </summary>
        private SteeringSystem _steering = new SteeringSystem();

        /// <summary>
        /// Turret component
        /// </summary>
        private Turret _turret;

        /// <summary>
        /// RigidBody
        /// </summary>
        private Rigidbody _rigidbody;

        /// <summary>
        /// Frontal speed
        /// </summary>
        [Header("Engine speeds")] [Range(5, 10)] [SerializeField]
        private int frontalVelocity = 5;

        /// <summary>
        /// Backward speed
        /// </summary>
        [Range(2, 5)] [SerializeField] private int backwardVelocity = 2;

        /// <summary>
        /// Rotation speed
        /// </summary>
        [Space] [Header("Body turn speed")] [Range(10, 30)] [SerializeField]
        private int rotationVelocity = 10;


        [Space] [Header("Turret rotation speed")] [Range(5, 10)] [SerializeField]
        private int turretRotationSpeed = 10;

        [SerializeField] private Rigidbody rotationBody;

        [Header("Gun rotator")] [SerializeField]
        private Transform gunRotator;

        [Header("Angles")] [Range(-5, 0)] [SerializeField]
        private float minRotationAngle;

        [Range(5, 10)] [SerializeField] private float maxRotationAngle;

       [SerializeField] private Camera _camera;

        public Camera Camera => _camera;
        #endregion
        
        #region Properties

        /// <summary>
        /// Accessor for frontal speed
        /// </summary>
        public int FrontalVelocity => frontalVelocity;

        /// <summary>
        /// Accessor for backward speed
        /// </summary>
        public int BackwardVelocity => backwardVelocity;

        /// <summary>
        /// Accessor for rotation speed
        /// </summary>
        public int RotationVelocity => rotationVelocity;

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

        public Transform GunRotator => gunRotator;

        #endregion

        #region BuiltInMethods

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.centerOfMass = Vector3.back;
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
            if (_rigidbody.velocity.magnitude <= velocity)
            {
                _engine.Move(_rigidbody, verticalDelta * velocity);
            }
        }

        /// <summary>
        /// Rotates tank body on a given euler rotation vector
        /// </summary>
        /// <param name="rotationAroundY"></param>
        public void TurnBody(Vector3 rotationAroundY)
        {
            var deltaRotation = Quaternion.Euler(rotationAroundY * Time.fixedDeltaTime);
            _steering.Rotate(_rigidbody, deltaRotation);
        }

        /// <summary>
        /// Stops tank
        /// </summary>
        public void Stop()
        {
            _engine.Stop(_rigidbody);
        }

        public void RotateTurret(float xAngle, float yAngle, float zAngle)
        {
            _turret.Rotate(rotationBody,
                new Quaternion(xAngle, yAngle * turretRotationSpeed * Time.deltaTime, zAngle, 1));
        }

        #endregion
    }
}