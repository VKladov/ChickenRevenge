using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Walking : MonoBehaviour
{
    [SerializeField] private Leg _leftLeg;
    [SerializeField] private Leg _rightLeg;
    [SerializeField] private float _speed = 1;

    private Vector3 _direction;

    private void Start()
    {
        _rightLeg.Fix(Vector3.zero, 0);
        _leftLeg.Bend();
    }

    private void Update()
    {
        if (_leftLeg.IsBended && _rightLeg.IsFixed && _rightLeg.NeedMove)
        {
            Debug.Log("Fix left");
            _leftLeg.Fix(_direction, _speed);
            _rightLeg.Bend();
        }
        else if (_rightLeg.IsBended && _leftLeg.IsFixed && _leftLeg.NeedMove)
        {
            Debug.Log("Fix right");
            _rightLeg.Fix(_direction, _speed);
            _leftLeg.Bend();
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        _direction = inputDirection;
    }

    private void FixedUpdate()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }
}
