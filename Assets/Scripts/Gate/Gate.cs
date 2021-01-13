using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Gate : DamageReceiver
{
    [SerializeField] private GateSlote[] _slots;
    [SerializeField] private MeshFilter _leftDoor;
    [SerializeField] private MeshFilter _rightDoor;
    [SerializeField] private Mesh[] _leftDoorMesh;
    [SerializeField] private Mesh[] _rightDoorMesh;
    [SerializeField] private Animator _brokenPartsAnimator;

    private BoxCollider _collider;
    private Dictionary<int, Mesh> _leftDoorHealthToMesh = new Dictionary<int, Mesh>();
    private Dictionary<int, Mesh> _rightDoorHealthToMesh = new Dictionary<int, Mesh>();

    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<BoxCollider>();

        if (_leftDoorMesh.Length != _rightDoorMesh.Length)
            throw new System.Exception();

        for (int i = 0; i < _leftDoorMesh.Length; i++)
        {
            int keyHealth = _health - (i + 1) * (_health / _leftDoorMesh.Length);
            _leftDoorHealthToMesh[keyHealth] = _leftDoorMesh[i];
            _rightDoorHealthToMesh[keyHealth] = _rightDoorMesh[i];
        }

        UpdateGatesMesh();
    }

    private void UpdateGatesMesh()
    {
        Mesh _leftDoorMesh = null;
        Mesh _rightDoorMesh = null;
        foreach (var healthMesh in _leftDoorHealthToMesh)
        {
            if (healthMesh.Key <= _health)
            {
                _leftDoorMesh = healthMesh.Value;
                _rightDoorMesh = _rightDoorHealthToMesh[healthMesh.Key];
                break;
            }
        }

        if (_leftDoorMesh != null)
            _leftDoor.mesh = _leftDoorMesh;

        if (_rightDoorMesh != null)
            _rightDoor.mesh = _rightDoorMesh;
    }

    public UnityAction<int> HealthChanged;

    public GateSlote GetSlote()
    {
        foreach (GateSlote oneSlote in _slots)
            if (oneSlote.IsEmpty)
                return oneSlote;

        return _slots[Random.Range(0, _slots.Length)];
    }

    public override void TakeDamage(int damage, bool fromPlayer = false)
    {
        base.TakeDamage(damage, fromPlayer);
        HealthChanged?.Invoke(_health);
        UpdateGatesMesh();
    }

    public override bool ShouldDestroy()
    {
        _collider.enabled = false;
        _leftDoor.gameObject.SetActive(false);
        _rightDoor.gameObject.SetActive(false);
        _brokenPartsAnimator.gameObject.SetActive(true);
        _brokenPartsAnimator.SetBool("isBroken", true);
        return false;
    }
}
