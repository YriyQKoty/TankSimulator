using System;
using System.Collections;
using System.Collections.Generic;
using MovementLogic;
using TurretSystem;
using UnityEngine;

public class TankUnit : MonoBehaviour
{
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
    private Turret _turret = new Turret();
    /// <summary>
    /// RigidBddy
    /// </summary>
    private Rigidbody _rigidbody;

    /// <summary>
    /// Frontal speed
    /// </summary>
    [Header("Engine speeds")]
    [Range(5, 10)] [SerializeField] private int frontalVelocity = 5;
    /// <summary>
    /// Rare speed
    /// </summary>
    [Range(2, 5)] [SerializeField] private int backwardVelocity = 2;

    /// <summary>
    /// Rotation speed
    /// </summary>
    [Space][Header("Body turn speed")]
    [Range(10, 30)] [SerializeField] private int rotationVelocity = 10;
    
    [Space][Header("Turret rotation speed")]
    [Range(5, 10)] [SerializeField] private int turretRotationSpeed = 10;

    [SerializeField] private Rigidbody rotationBody;

    /// <summary>
    /// Accessor for rotation speed
    /// </summary>
    public int RotationVelocity => rotationVelocity;

    /// <summary>
    /// Accessor for frontal speed
    /// </summary>
    public int FrontalVelocity => frontalVelocity;

    /// <summary>
    /// Accessor for backward speed
    /// </summary>
    public int BackwardVelocity => backwardVelocity;

    private float _currentVelocity;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = Vector3.back;
    }
    
    /// <summary>
    ///  Moves a tank with corresponding velocity
    /// </summary>
    /// <param name="verticalDelta">value of "Vertical" axis for WS<!--<>--></param>
    /// <param name="velocity">speed</param>
    public void Move(float verticalDelta, float velocity)
    {
        _currentVelocity = velocity;
        if (_rigidbody.velocity.magnitude <= _currentVelocity)
        {
            _engine.Move(_rigidbody, verticalDelta * _currentVelocity);
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
        _turret.Rotate(rotationBody, new Quaternion(xAngle,yAngle*turretRotationSpeed * Time.deltaTime,zAngle,1));
    }
    
}
