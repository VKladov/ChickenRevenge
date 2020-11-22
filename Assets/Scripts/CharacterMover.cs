using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterMover : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] float _jumpSpeed = 0.5f;
    [SerializeField] float _gravity = -2f;
    [SerializeField] GameField _field;

    private CharacterController _characterController;
    private Animator _animator;
    private Vector3 _moveDirection = Vector3.zero;
    private bool _jumped => _characterController.isGrounded && Input.GetKey(KeyCode.Space);
    private bool _hasTarget = false;
    private Vector3 _positionWhenSetTarget;
    private Vector3 _target;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_hasTarget)
        {
            MoveToTarget();
            if (Mathf.Abs(Vector3.Distance(_positionWhenSetTarget, transform.position) - Vector3.Distance(_positionWhenSetTarget, _target)) < 0.1f)
                _hasTarget = false;
        }
        else
            Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 transformDirection = transform.TransformDirection(inputDirection);

        Vector3 flatMovement = _moveSpeed * Time.deltaTime * transformDirection;
        _moveDirection = new Vector3(flatMovement.x, _moveDirection.y, flatMovement.z);

        if (_jumped)
        {
            _moveDirection.y = _jumpSpeed;
            _animator.SetTrigger("jump");
        }
        else if (_characterController.isGrounded)
            _moveDirection.y = 0f;
        else
            _moveDirection.y -= _gravity * Time.deltaTime;


        _characterController.Move(_moveDirection);
        _animator.SetFloat("walkSpeed", inputDirection.magnitude > 0 ? _moveSpeed : 0);
        _animator.SetFloat("movementX", inputDirection.normalized.x);
        _animator.SetFloat("movementY", inputDirection.normalized.z);
    }

    private void MoveToTarget()
    {
        Vector3 direction = _target - transform.position;
        _moveDirection = direction * _moveSpeed * Time.deltaTime;

        transform.position += _moveDirection;
        _animator.SetFloat("walkSpeed", _moveSpeed);
        _animator.SetFloat("movementX", 0);
        _animator.SetFloat("movementY", 1);
    }

    public void SetTarget(Vector3 target)
    {
        _positionWhenSetTarget = transform.position;
        _target = target;
        _hasTarget = true;
        transform.rotation = Quaternion.LookRotation(target - transform.position, transform.up);
    }
}
