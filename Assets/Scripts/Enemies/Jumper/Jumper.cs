using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Jumper : Enemy
{
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpDelay;
    [SerializeField] private float _jumpDuration;
    [SerializeField] private float _jumpDistance;
    [SerializeField] private float _jumpDistanceSpread;
    [SerializeField] private DamageReceiver _damageReceiver;

    private float _currentTime = 0;
    private Vector3 _jumpPoint;
    private Vector3 _targetPoint;
    private Vector3 _jumpVelocity;
    private Animator _animator;
    private Coroutine _jumpWork;
    private Vector3 _direction;
    private float _minZ;
    private float _maxZ;

    public void Init(Vector3 direction, float minZ, float maxZ)
    {
        _direction = direction;
        _minZ = minZ;
        _maxZ = maxZ;
        Jump(GetRandomTargetPoint());
    }

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _damageReceiver.Disappeared += OnDisappeared;
        _damageReceiver.Killed += OnKilled;
        _damageReceiver.Destroyed += OnDestroyed;
    }

    private void OnDisable()
    {
        _damageReceiver.Disappeared -= OnDisappeared;
        _damageReceiver.Killed -= OnKilled;
        _damageReceiver.Destroyed -= OnDestroyed;
    }

    private void OnDisappeared(DamageReceiver damageReceiver) => Destroy(gameObject);

    private void OnKilled(DamageReceiver damageReceiver) => Killed?.Invoke(this);

    private void OnDestroyed(DamageReceiver damageReceiver) => Destroyed?.Invoke(this);

    private void Jump(Vector3 target)
    {
        Sounds.main.PlayJumperJump(transform.position);
        Effects.main.PlayEffect(transform.position, ParticleEffectName.PapuanDestroy);
        _jumpPoint = transform.position;
        _targetPoint = target;
        _jumpVelocity = CalculateVelocity(_targetPoint, _jumpPoint, _jumpDuration);
        _currentTime = 0;
    }

    private void FixedUpdate()
    {
        if (_currentTime > _jumpDuration)
        {
            if (_jumpWork == null)
                _jumpWork = StartCoroutine(JumpAfter(_jumpDelay));
        }
        else
        {
            transform.position = CalculatePositionInTime(_jumpVelocity, _currentTime);
            _currentTime += Time.deltaTime;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_targetPoint - _jumpPoint, Vector3.up), Time.deltaTime);
    }

    private IEnumerator JumpAfter(float delay)
    {
        _animator.SetTrigger("jump");
        yield return new WaitForSeconds(delay);
        Jump(GetRandomTargetPoint());
        _jumpWork = null;
    }

    private Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float sY = distance.y;
        float sXZ = distanceXZ.magnitude;

        float Vxz = sXZ / time;
        float Vy = (sY / time) + (0.5f * _gravity * time);

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;
        return result;
    }

    private Vector3 CalculatePositionInTime(Vector3 vo, float time)
    {
        Vector3 vXz = vo;
        vXz.y = 0;

        Vector3 result = _jumpPoint + vo * time;
        float sY = (-0.5f * _gravity * time * time) + vo.y * time + _jumpPoint.y;
        result.y = sY;

        return result;
    }

    private Vector3 GetRandomTargetPoint()
    {
        float movementX = _direction.x > 0 ? Random.Range(0f, 1f) : Random.Range(-1f, 0f);
        float distance = Random.Range(_jumpDistance - _jumpDistanceSpread / 2, _jumpDistance + _jumpDistanceSpread / 2);
        Vector3 movement = new Vector3(movementX, 0, Random.Range(-1f, 1f)).normalized * distance;
        Vector3 target = transform.position + movement;
        target.y = 0;
        target.z = Mathf.Max(_minZ, Mathf.Min(_maxZ, target.z));

        return target;
    }
}
