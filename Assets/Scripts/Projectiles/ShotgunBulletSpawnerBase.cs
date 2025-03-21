using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletSpawnerBase : ProjectileBase
{


    [SerializeField] private int numBullets = 5; // Number of bullets to spawn
    [SerializeField] private float angleVariation = 15f; // Max angle variation in degrees
    [SerializeField] private GameObject bulletPrefab; // The bullet prefab to spawn

    private void SpawnBullets()
    {
        for (int i = 0; i < numBullets; i++)
        {
            float angleOffset = Random.Range(-angleVariation, angleVariation); // Random offset
            Vector3 spawnDirection = Quaternion.Euler(0, 0, angleOffset) * direction; // Rotate the original direction

            GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            ProjectileBase bulletScript = newBullet.GetComponent<ProjectileBase>();
            bulletScript.SetTarget(target, spawnDirection.normalized);
        }
    }

    public override void SetTarget(Transform _target, Vector2 _direction)
    {
        base.SetTarget(_target, _direction);
        DestroyProjectile();
    }

    protected override void DestroyProjectile()
    {
        SpawnBullets();

        base.DestroyProjectile();
    }
}
