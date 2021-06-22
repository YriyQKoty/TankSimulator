using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField]private Collider _collider;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private ParticleSystem _system;
    
    
    private void OnCollisionEnter(Collision collision)
    {
        _system.Play();
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _collider.isTrigger = false;
        _renderer.enabled = false;
        _source.Play();
        Destroy(gameObject, 3);
    }

    
}
