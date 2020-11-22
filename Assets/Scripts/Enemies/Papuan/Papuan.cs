using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Papuan : DamageReceiver
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private GameObject _fork;

    private Animator _animator;
    private PapuanSpawner _spawner;

    private Vector3 _direction;
    private Vector3 _targetPosition;
    private bool _isGoingDown = true;
    private bool _canRunDown = false;
    private Papuan _leader = null;
    private bool _isLeader = false;
    private bool _caughtChicken = false;

    public int Row { get; protected set; }
    public int Col { get; protected set; }
    public Vector3 TargetPosition => _targetPosition;

    public void Init(PapuanSpawner spawner, int row, int col, Papuan leader)
    {
        _spawner = spawner;
        Row = row;
        Col = col;
        _leader = leader;
        if (_leader != null)
            _leader.Disappeared += OnLeaderDied;

        _targetPosition = transform.position;
        SetNextTargetPosition();
    }

    private void OnDisable()
    {
        if (_leader != null)
            _leader.Disappeared -= OnLeaderDied;
    }

    override protected void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _fork.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_caughtChicken)
            return;

        if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
            SetNextTargetPosition();

        Move();
    }

    public void RunDown(float targetZ)
    {
        if (_canRunDown || _caughtChicken) { return; }
        _canRunDown = true;
        _speed *= Random.Range(2f, 2.5f);
        _targetPosition = new Vector3(transform.position.x, 0, targetZ);
    }

    public void Move()
    {
        _direction = (_targetPosition - transform.position).normalized;

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_direction, transform.up), Time.deltaTime * 10);
        _animator.SetFloat("walkSpeed", _speed);
    }

    public void SetNextTargetPosition()
    {
        if (_canRunDown)
        {
            Destroy();
            return;
        }

        if (_targetPosition.magnitude > 0)
            transform.position = _targetPosition;

        _spawner.GetAllowedDirections(Row, Col, out bool left, out bool right);
        if (_direction.x < 0 && !left || _direction.x > 0 && !right || (!left && !right && _direction.x == 0))
        {
            if (Row == _spawner.MinRow)
                _isGoingDown = false;
            if (Row == _spawner.MinRow + _spawner.PlayerRows)
                _isGoingDown = true;

            Row += _isGoingDown ? -1 : 1;
        }
        else if (left && right)
        {
            if (_direction.x == 0)
                Col += -1;
            else
                Col += _direction.x < 0 ? -1 : 1;
        }
        else if (left)
        {
            Col--;
        }
        else if (right)
        {
            Col++;
        }

        _targetPosition = _spawner.GetPosition(Row, Col);

        if (_leader == null || (_leader != null && _leader.TargetPosition != _targetPosition))
            BecomeLeader();
    }

    public void TakeDamage()
    {
        Killed?.Invoke(this);
        Destroy(gameObject);
    }

    public void BecomeLeader()
    {
        if (_isLeader)
            return;

        _isLeader = true;
        _fork.SetActive(true);
        _animator.SetBool("IsLeader", true);
    }

    private void OnLeaderDied(DamageReceiver leader)
    {
        BecomeLeader();
    }

    public void CatchPlayer(Player player)
    {
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
        BecomeLeader();
        _caughtChicken = true;
        _animator.SetTrigger("attack");
    }
}
