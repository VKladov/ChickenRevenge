using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanGun : Gun
{
    [SerializeField] private BulletTrail _trailPrefab;
    [SerializeField] private float _maxDistance;
    [SerializeField] private int _damage;
    [SerializeField] private Camera _aimingCamera;

    protected override void Shoot(Transform shootPoint)
    {
        Vector3 trailTarget = shootPoint.transform.position + shootPoint.forward * _maxDistance;
        if (Physics.Raycast(new Ray(_aimingCamera.transform.position, _aimingCamera.transform.forward), out RaycastHit target, _maxDistance))
        {
            if (target.collider.gameObject.TryGetComponent(out DamageReceiver destroyable))
                destroyable.TakeShot(_damage);
        }

        BulletTrail trail = Instantiate(_trailPrefab, shootPoint.position, Quaternion.identity);
        trail.Init(trailTarget);
    }
}
