using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanStandingInGateSlote : PapuanState
{
    public PapuanStandingInGateSlote(Papuan papuan) : base(papuan) { }

    public override void Enter()
    {
        Papuan.Animator.SetFloat("walkSpeed", 0);
        if (Papuan.TargetSlote != null)
            Papuan.RotateTo(-Papuan.TargetSlote.transform.forward);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
