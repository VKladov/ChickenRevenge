using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bonus : DamageReceiver
{
    [SerializeField] private BonusName[] _bonuses;
    [SerializeField] private float _rotationSpeed = 50;
    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private float _bounce = 1;
    [SerializeField] private float _minJumpLength = 0.1f;
    [SerializeField] private float _destroyDelay = 0.5f;
    [SerializeField] private float _explosionRadius = 1;

    private Vector3 _targetDirection;
    private float _targetDistance;
    private float _currentJumpTime;
    private Vector3 _forceDirection;
    private float _a = Mathf.PI / 4.0f;

    public BonusName BonusName => _bonuses[Random.Range(0, _bonuses.Length)];
    public float ExplosionRadius => _explosionRadius;

    private void Update()
    {
        float rotationDelta = _rotationSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(rotationDelta, rotationDelta, rotationDelta));
        if (_targetDistance < 0.01f)
            return;

        if (transform.position.y >= 0)
            UpdatePosition();
        else
        {
            if (_targetDistance * _bounce >= _minJumpLength)
                SetJumpTarget(transform.position + _targetDirection * _targetDistance * _bounce);
            else
                StartCoroutine(DestroyAfter(_destroyDelay));
        }
    }

    public void SetJumpTarget(Vector3 target)
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        _targetDirection = (target - transform.position).normalized;
        _targetDistance = Vector3.Distance(target, transform.position);

        float v0 = Mathf.Sqrt((_targetDistance * _gravity) / (Mathf.Sign(_a * 2)));
        float angle = Vector3.Angle(new Vector3(1, 0, 0), new Vector3(_targetDirection.x, 0, _targetDirection.z));
        if (_targetDirection.z < 0)
            angle *= -1;

        _forceDirection = new Vector3(v0 * Mathf.Cos(_a) * Mathf.Cos(angle / 180 * Mathf.PI), v0 * Mathf.Sin(_a), v0 * Mathf.Cos(_a) * Mathf.Sin(angle / 180 * Mathf.PI));

        _currentJumpTime = 0;
    }

    private void UpdatePosition()
    {
        float speedX = _forceDirection.x;
        float speedZ = _forceDirection.z;
        float speedY = _forceDirection.y - _gravity * _currentJumpTime;

        transform.position += new Vector3(speedX * Time.deltaTime, speedY * Time.deltaTime, speedZ * Time.deltaTime);

        _currentJumpTime += Time.deltaTime;
    }

    private IEnumerator DestroyAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
