using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : Enemy
{
    [SerializeField] private RollerWheel _wheel;
    [SerializeField] private RollerDriver _driver;
    [SerializeField] private float _speed;
    [SerializeField] private float _sideSpeed;
    [SerializeField] private LayerMask _groundMask;

    private GameObject _target;
    private Vector3 _direction;
    private Camera _camera;

    public Vector3 PositionInCamera { get; private set; }
    public bool VisibleInCamera { get; private set; } = false;

    override protected void Awake()
    {
        _wheel.GetComponent<Animator>().SetFloat("speed", _speed);
        _driver.GetComponent<Animator>().SetFloat("speed", _speed);
        _camera = FindObjectOfType<Camera>();
    }

    private void OnEnable()
    {
        _wheel.Destroyed += OnPartDestroyed;
        _driver.Destroyed += OnPartDestroyed;
    }

    private void OnDisable()
    {
        _wheel.Destroyed -= OnPartDestroyed;
        _driver.Destroyed -= OnPartDestroyed;
    }

    private void OnPartDestroyed(DamageReceiver part) => Destroy();

    public void Init(Vector3 direction, GameObject target)
    {
        _direction = direction;
        _target = target;
        transform.rotation = Quaternion.LookRotation(_direction, transform.up);
    }

    private void FixedUpdate()
    {
        Move();
        PositionInCamera = _camera.WorldToViewportPoint(transform.position);
        VisibleInCamera = PositionInCamera.x >= 0 && PositionInCamera.x <= 1 &&
            PositionInCamera.y >= 0 && PositionInCamera.y <= 1 &&
            PositionInCamera.z > 0;
    }

    private void Move()
    {
        Vector3 nextPosition;
        Vector3 movement;
        if (_target != null)
        {
            Vector3 sideMovement = Vector3.zero;
            if (Mathf.Abs(_target.transform.position.z - transform.position.z) > 0.1f)
            {
                bool toBottom = _target.transform.position.z < transform.position.z;
                sideMovement = new Vector3(0, 0, toBottom ? -1 : 1) * _sideSpeed * Time.deltaTime;
            }

            movement = _direction * _speed * Time.deltaTime + sideMovement;
        }
        else
        {
            movement = _direction * _speed * Time.deltaTime;
        }

        nextPosition = transform.position + movement;


        if (Physics.Raycast(nextPosition + Vector3.up * 3, Vector3.down, out RaycastHit hit, 10, _groundMask))
        {
            nextPosition = hit.point;
            transform.up = hit.normal;
        }

        transform.position = nextPosition;
        transform.rotation = Quaternion.LookRotation(movement, transform.up);
    }
}
