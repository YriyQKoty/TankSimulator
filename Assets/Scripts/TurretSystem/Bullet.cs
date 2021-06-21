using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField]private Collider _collider;
    [SerializeField] private MeshRenderer _renderer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _collider.isTrigger = false;
            _renderer.enabled = false;
            _source.Play();
            Destroy(gameObject, 3);
        }
       
    }

    
}
