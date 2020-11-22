using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage = 1;

    private float _bounceDistance = 0.01f;
    private Vector3 _direction;
    private Vector3 _lastBouncePoint;
    private GameObject _lastTrigger;

    public int Damage => _damage;

    public void Init(Vector3 direction)
    {
        _direction = direction;
        _lastBouncePoint = transform.position;
    }

    public void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0 && Vector3.Distance(_lastBouncePoint, transform.position) > _bounceDistance)
        {
            _lastBouncePoint = transform.position;
            _direction = Vector3.Reflect(_direction, collision.contacts[0].normal);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_lastTrigger == other.gameObject)
        {
            return;
        }

        _lastTrigger = other.gameObject;
        if (other.TryGetComponent(out Cactus cactus))
        {
            cactus.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}