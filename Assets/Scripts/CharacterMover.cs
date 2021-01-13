using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class CharacterMover : MonoBehaviour
{
    [SerializeField] private GameInput _input;
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] float _jumpSpeed = 0.5f;
    [SerializeField] float _gravity = -2f;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private Vector3 _moveDirection = Vector3.zero;
    private bool _jumped => _input.IsJumpPressed;
    private Vector3 _target;
    private bool _isGrounded = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 inputDirection = new Vector3(_input.Movement.x, 0, _input.Movement.y).normalized;
        Vector3 transformDirection = transform.TransformDirection(inputDirection);

        Vector3 flatMovement = _moveSpeed * Time.deltaTime * transformDirection;
        _moveDirection = new Vector3(flatMovement.x, _moveDirection.y, flatMovement.z);

        if (_jumped)
        {
            _moveDirection.y = _jumpSpeed;
            _animator.SetTrigger("jump");
        }
        else if (_isGrounded)
            _moveDirection.y = 0f;
        else
            _moveDirection.y -= _gravity * Time.deltaTime;


        _rigidbody.MovePosition(transform.position + _moveDirection);
        _animator.SetFloat("walkSpeed", inputDirection.magnitude > 0 ? _moveSpeed : 0);
        _animator.SetFloat("movementX", inputDirection.normalized.x);
        _animator.SetFloat("movementY", inputDirection.normalized.z);
    }
}
