using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanMoveToGateSloteState : PapuanState
{
    public PapuanMoveToGateSloteState(Papuan papuan) : base(papuan) { }

    public override void Enter()
    {
        Papuan.Animator.SetFloat("walkSpeed", Papuan.Speed);
        Papuan.TargetSlote.MakeBusy(Papuan);
    }

    public override void Exit() { }

    public override void Update()
    {
        Papuan.Move((Papuan.TargetSlote.transform.position - Papuan.transform.position).normalized);
    }
}
