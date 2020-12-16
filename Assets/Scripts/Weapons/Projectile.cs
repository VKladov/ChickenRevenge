using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private float _gravity;

    private Vector3 _initialSpeed;
    private Vector3 _shootPoint;
    private float _shootTime;

    private void FixedUpdate()
    {
        if (_initialSpeed != Vector3.zero)
            Move();
    }

    public void SetSpeed(Vector3 speed)
    {
        _initialSpeed = speed;
        _shootPoint = transform.position;
        _shootTime = 0;
    }

    private void Move()
    {
        Vector3 vXz = _initialSpeed;
        vXz.y = 0;

        Vector3 nextPosition = _shootPoint + _initialSpeed * _shootTime;
        float sY = (-0.5f * _gravity * _shootTime * _shootTime) + _initialSpeed.y * _shootTime + _shootPoint.y;
        nextPosition.y = sY;

        Vector3 moveDirection = nextPosition - transform.position;

        transform.position = nextPosition;
        transform.rotation = Quaternion.LookRotation(moveDirection, transform.up);

        _shootTime += Time.deltaTime;
    }
}
