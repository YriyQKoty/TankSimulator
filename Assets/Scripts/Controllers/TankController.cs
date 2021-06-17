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

        [Range(5, 25)] [SerializeField] private float maxRotationAngle;

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

        /// <summary>
        /// Accessor for gun rotator
        /// </summary>
        public Transform GunRotator => gunRotator;

        #endregion

        #region BuiltInMethods

        private void Start()
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
            if (_rigidbody.velocity.magnitude <= velocity)
            {
                //give a command to engine to move rigidbody with a speed as vertical input (W/S keys) multiplied by velocity
                _trackSystemController.Move(verticalDelta * velocity);
            }
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
            _trackSystemController.Stop(_rigidbody);
        }

        /// <summary>
        /// Rotates turret with given angles
        /// </summary>
        /// <param name="xAngle"></param>
        /// <param name="yAngle"></param>
        /// <param name="zAngle"></param>
        public void RotateTurret(Vector3 eulerAngles)
        {
            _turret.Rotate(rotationBody,
                new Quaternion(eulerAngles.x, eulerAngles.y * turretRotationSpeed * Time.deltaTime, eulerAngles.z, 1));
        }

        #endregion
    }
}