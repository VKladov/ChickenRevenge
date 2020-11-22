using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Leg : MonoBehaviour
{
    [SerializeField] private GameObject _foot;
    [SerializeField] private float _maxDeltaMultiplier = 1.5f;
    [SerializeField] private LayerMask _groundLayer;

    private float _moveDuration = 0.1f;

    public float Length { get; private set; }
    public bool NeedMove => Vector2.Distance(
        new Vector2(_foot.transform.position.x, _foot.transform.position.z),
        new Vector2(transform.position.x, transform.position.z)
    ) > _maxDelta;
    public bool IsFixed { get; private set; } = false;
    public bool IsBended { get; private set; } = false;
    public bool IsMoving { get; private set; } = false;

    private float _maxDelta;
    private Vector3 _fixPoint;

    private void Awake()
    {
        Length = Vector3.Distance(_foot.transform.position, transform.position);
        _maxDelta = Length * _maxDeltaMultiplier;
    }

    public void Bend()
    {
        IsBended = true;
        IsFixed = false;

        _foot.transform.DOMove(transform.position + Vector3.down * Length * 0.8f, _moveDuration);
    }

    public void Fix(Vector3 _direction, float speed)
    {
        Vector3 rayOrigin = transform.position + _direction * _maxDelta * 0.8f;
        Vector3 delayDelta = _direction * speed * _moveDuration * 0.8f;
        if (Physics.Raycast(new Ray(rayOrigin + delayDelta, Vector3.down), out RaycastHit hit, Length * 2, _groundLayer))
        {
            _fixPoint = hit.point;
            //_foot.transform.position = _fixPoint;
            IsFixed = true;
            IsBended = false;

            _foot.transform.DOMove(_fixPoint, _moveDuration);
        }
    }

    private void Update()
    {
        if (IsFixed)
            _foot.transform.position = _fixPoint;
    }
}
