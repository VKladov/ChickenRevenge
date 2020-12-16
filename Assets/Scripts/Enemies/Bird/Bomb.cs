using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _acceleration;
    [SerializeField] private int _damage;
    [SerializeField] private float _explosionRadius;

    private void OnTriggerEnter(Collider other) => Explode();
    private Vector3 _speed = Vector3.zero;

    public void Init(Vector3 speed)
    {
        _speed = speed;
    }

    private void FixedUpdate()
    {
        _speed += Vector3.down * _acceleration * Time.deltaTime;
        transform.position += _speed * Time.deltaTime;
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
