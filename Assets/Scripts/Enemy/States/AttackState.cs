using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PapuanState
{
    private float _attackDelay;
    protected float _timePassed = 0;
    protected bool _attacked = false;
    public AttackState(Papuan papuan, float attackDelay) : base(papuan)
    {
        _attackDelay = attackDelay;
    }

    public override void Enter()
    {
        Papuan.Animator.SetTrigger("attack");
        if (Papuan.TargetEnemy != null)
            Papuan.RotateTo(Papuan.TargetEnemy.transform.position - Papuan.transform.position);

        _attacked = false;
        _timePassed = 0;
    }

    public override void Exit() { }

    public override void Update()
    {
        _timePassed += Time.deltaTime;
        if (!_attacked && _timePassed >= _attackDelay)
            Attack();
    }

    private void Attack()
    {
        _attacked = true;

        if (Physics.Raycast(Papuan.transform.position + Vector3.up * 0.5f, Papuan.transform.forward, out RaycastHit hit, 2f))
        {
            if (hit.collider.TryGetComponent(out DamageReceiver receiver))
            {
                receiver.TakeHit(1, Papuan.transform.forward);
                if (receiver is Character)
                    (receiver as Character).LastDamageSource = Papuan;
            }
        }
    }
}
