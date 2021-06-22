using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public ParticleSystem[] smokeEffects;
    public GameObject bulletPrefab;

    private bool _canShoot = true;

    public bool CanShoot => _canShoot;

    private int timer = 0;
    private int force = 250;
    
    public int timeBetweenShot = 6;

    public Transform spawnPoint;

    public void Fire()
    {
        StartCoroutine(Shoot());
    }
    
    private IEnumerator Shoot()
    {
        foreach (var system in smokeEffects)
        {
            system.Play();
        }
        
        var bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<Rigidbody>();

        bullet.velocity = force * spawnPoint.forward;

        _canShoot = false;

        timer = timeBetweenShot;

        while (timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer--;
        }

        _canShoot = true;
    }

    private void Update()
    {
        RaycastHit hit; // declare the RaycastHit variable
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
   
        if (Physics.Raycast(ray, out hit)) {
            Debug.DrawRay(hit.transform.position, spawnPoint.forward, Color.green);
        }
    }
}
