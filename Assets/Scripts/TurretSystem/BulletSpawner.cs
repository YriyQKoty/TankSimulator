using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;

    private bool _canShoot = true;

    public bool CanShoot => _canShoot;

    private int timer = 0;
    private int force = 100;
    
    public int timeBetweenShot = 6;

    public Transform spawnPoint;

    public void Fire()
    {
        StartCoroutine(Shoot());
    }
    
    private IEnumerator Shoot()
    {
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
}
