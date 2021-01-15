using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanLookAtPlayerState : PapuanState
{
    public PapuanLookAtPlayerState(Papuan papuan) : base(papuan) { }

    public override void Enter()
    {
        Papuan.Animator.SetFloat("walkSpeed", 0);
    }

    public override void Exit() { }

    public override void Update()
    {
        Papuan.RotateTo(Papuan.TargetEnemy.transform.position - Papuan.transform.position);
    }
}
