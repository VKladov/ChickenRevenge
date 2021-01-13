using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBackState : PapuanState
{
    public AttackBackState(Papuan papuan) : base(papuan) { }

    public override void Enter()
    {
        if (Papuan.LastDamageSource != null)
        {
            Papuan.SetTargetEnemy(Papuan.LastDamageSource);
            Papuan.LastDamageSource = null;
            Papuan.RotateTo(Papuan.LastDamageSource.transform.position - Papuan.transform.position);
        }
    }

    public override void Exit() { }

    public override void Update() { }
}
