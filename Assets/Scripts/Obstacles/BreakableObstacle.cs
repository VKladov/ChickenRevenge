using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class BreakableObstacle : Obstacle
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private Mesh[] _meshes;

    private Mesh _defaultMesh;

    protected override void Awake()
    {
        base.Awake();
        _defaultMesh = _meshFilter.mesh;
    }

    public override void TakeDamage(int damage, bool fromPlayer = false)
    {
        base.TakeDamage(damage, fromPlayer);
        UpdateMesh();
    }

    public override void Recover()
    {
        base.Recover();
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        if (_health <= 0)
            _meshFilter.mesh = null;

        else if (_health == _maxHealth)
            _meshFilter.mesh = _defaultMesh;

        else if (_meshes.Length > _health - 1)
            _meshFilter.mesh = _meshes[_health - 1];
    }
}
