using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanGun : Gun
{
    [SerializeField] private BulletTrail _trailPrefab;
    [SerializeField] private float _maxDistance;
    [SerializeField] private int _damage;

    protected override void Shoot(Transform shootPoint)
    {
        Vector3 trailTarget;
        if (Physics.Raycast(new Ray(shootPoint.transform.position, shootPoint.forward), out RaycastHit target, _maxDistance))
        {
            if (target.collider.gameObject.TryGetComponent(out DamageReceiver destroyable))
                destroyable.TakeDamage(_damage, true);

            if (target.collider.gameObject.TryGetComponent(out StonePiece stone))
                Destroy(stone.gameObject);

            if (target.collider.gameObject.TryGetComponent(out ShootableObject shootable))
                shootable.TakeShoot(target.point, shootPoint.forward);

            trailTarget = target.point;
        }
        else
        {
            trailTarget = shootPoint.transform.position + shootPoint.forward * _maxDistance;
        }

        BulletTrail trail = Instantiate(_trailPrefab, shootPoint.position, Quaternion.identity);
        trail.Init(trailTarget);
    }
}
