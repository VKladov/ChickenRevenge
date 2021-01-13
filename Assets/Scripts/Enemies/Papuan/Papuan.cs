using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Papuan : Character
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private GameObject _fork;
    [SerializeField] private float _viewDistance;
    [SerializeField] private Transform _viewPoint;
    [SerializeField] private float _fieldOfView;
    [SerializeField] private MeshRenderer _teamMark;

    public Animator Animator { get; private set; }
    public float Speed => _speed;
    public float ViewDistance => _viewDistance;
    public Transform ViewPoint => _viewPoint;
    public Gate TargetGate { get; private set; }
    public GateSlote TargetSlote;

    private Rigidbody _rigidbody;
    private Character _player;

    public bool CouldSeeCharacter(Character character)
    {
        if (Vector3.Distance(transform.position, character.transform.position) > ViewDistance)
                return false;

            float angle = Vector3.Angle(ViewPoint.forward, character.transform.position - transform.position);

            if (angle > _fieldOfView / 2)
                return false;

            return true;
    }

    public void Init(Gate targetGate, Character player)
    {
        TargetGate = targetGate;
        TargetSlote = targetGate.GetSlote();
        _player = player;
    }

    override protected void Awake()
    {
        base.Awake();
        Animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        Vector3 movement = direction * _speed * Time.deltaTime;
        _rigidbody.MovePosition(transform.position + movement);
        RotateTo(direction);
        Animator.SetFloat("walkSpeed", _speed);
    }

    public void RotateTo(Vector3 direction)
    {
        direction.y = 0;
        _rigidbody.MoveRotation(Quaternion.LookRotation(direction, transform.up));
    }

    public void SetMarkMaterial(Material material)
    {
        _teamMark.material = material;
    }

    public override void TakeShot(int damage)
    {
        base.TakeShot(damage);

        LastDamageSource = _player;
    }
}
