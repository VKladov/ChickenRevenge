using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Projectile
{
    [SerializeField] private int _damage;
    [SerializeField] private float _explosionRadius;

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    private void Explode()
    {
        RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position + Vector3.up * 0.1f, _explosionRadius, Vector3.down);
        foreach (RaycastHit hit in raycastHits)
            if (hit.transform.TryGetComponent(out DamageReceiver destroyable))
                destroyable.TakeDamage(_damage);

        Sounds.main.PlayExplosion(transform.position);
        Effects.main.PlayEffect(transform.position, ParticleEffectName.CoinDestroy);
        Destroy(gameObject);
    }
}
