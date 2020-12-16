using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : Gun
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _shotSpeed;

    protected override void Shoot(Transform shootPoint)
    {
        Projectile bullet = Instantiate(_projectilePrefab, shootPoint.position, Quaternion.identity);
        bullet.SetSpeed(shootPoint.forward * _shotSpeed);
    }
}
