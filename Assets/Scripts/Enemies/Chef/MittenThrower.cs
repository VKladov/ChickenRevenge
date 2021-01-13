using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MittenThrower : MonoBehaviour
{
    [SerializeField] private GameObject _mitten;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private Transform _mittenBone;
    private float _currentMoveSpeed;
    private float _currentRotationSpeed;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Vector3 _target;
    private bool _isFlying = false;
    private bool _isFlyingBack = false;

    public bool HasMitten => _mitten.transform.parent == _mittenBone;
    public UnityAction Сaught;

    private void Start()
    {
        _initialPosition = _mitten.transform.localPosition;
        _initialRotation = _mitten.transform.localRotation;
        _mittenBone = _mitten.transform.parent;
    }

    private void Update()
    {
        if (_isFlying == false)
            return;

        _mitten.transform.position = Vector3.MoveTowards(_mitten.transform.position, _target, _currentMoveSpeed * Time.deltaTime);
        _mitten.transform.Rotate(Vector3.forward, _currentRotationSpeed * Time.deltaTime);

        if (Vector3.Distance(_target, _mitten.transform.position) < 0.1f)
        {
            if (_isFlyingBack)
            {
                Catch();
            }
            else
            {
                _target = _mittenBone.transform.position;
                _isFlyingBack = true;
            }
        }
    }

    public void Throw(Vector3 target)
    {
        _isFlying = true;
        _isFlyingBack = false;
        _target = target;
        _mitten.transform.parent = null;
        _currentMoveSpeed = _moveSpeed;
        _currentRotationSpeed = _rotationSpeed;
    }

    public void Catch()
    {
        _isFlying = false;
        _currentMoveSpeed = 0;
        _currentRotationSpeed = 0;
        _mitten.transform.parent = _mittenBone;
        _mitten.transform.localPosition = _initialPosition;
        _mitten.transform.localRotation = _initialRotation;
        Сaught?.Invoke();
    }
}
