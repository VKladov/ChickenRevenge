using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanMoveState : PapuanState
{
    public PapuanMoveState(Papuan papuan) : base(papuan) { }

    public override void Enter()
    {
        Papuan.Animator.SetFloat("walkSpeed", Papuan.Speed);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (Papuan.TargetEnemy != null)
            Papuan.Move((Papuan.TargetEnemy.transform.position - Papuan.transform.position).normalized);
    }
}
