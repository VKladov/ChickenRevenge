using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Woman : Enemy
{
    [SerializeField] private float _speed;
    [SerializeField] private float _sideSpeed;
    [SerializeField] private LayerMask _groundMask;

    private Animator _animator;
    private GameObject _target;
    private Vector3 _direction = Vector3.back;

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    override protected void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _animator.SetFloat("speed", _speed);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 nextPosition;
        Vector3 movement;
        if (_target != null)
        {
            Vector3 sideMovement = Vector3.zero;
            if (Mathf.Abs(_target.transform.position.x - transform.position.x) > 0.1f)
            {
                bool toLeft = _target.transform.position.x < transform.position.x;
                sideMovement = new Vector3(toLeft ? -1 : 1, 0, 0) * _sideSpeed * Time.deltaTime;
            }

            movement = _direction * _speed * Time.deltaTime + sideMovement;
        }
        else
        {
            movement = _direction * _speed * Time.deltaTime;
        }

        nextPosition = transform.position + movement;

        Vector3 normal = Vector3.up;
        if (Physics.Raycast(nextPosition + Vector3.up * 3, Vector3.down, out RaycastHit hit, 10, _groundMask))
        {
            nextPosition = hit.point;
            transform.up = hit.normal;
        }

        transform.position = nextPosition;
        transform.rotation = Quaternion.LookRotation(movement, transform.up);
    }

    public override void TakeDamage(int damage, bool fromPlayer = false)
    {
        base.TakeDamage(damage, fromPlayer);

        if (_health > 0)
            Sounds.main.PlaySoftHit(transform.position);
    }
}
