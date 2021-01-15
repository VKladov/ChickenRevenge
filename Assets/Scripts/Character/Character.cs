using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : DamageReceiver
{
    private Rigidbody _rigidbody;

    public Character TargetEnemy { get; private set; }
    public Character LastDamageSource;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetTargetEnemy(Character enemy)
    {
        TargetEnemy = enemy;
    }

    public override void TakeHit(int damage, Vector3 direction, bool fromPlayer = false)
    {
        if (Health > 0)
            _rigidbody.AddForce((direction + Vector3.up) * damage * 2, ForceMode.Impulse);

        base.TakeHit(damage, direction, fromPlayer);
    }
}
