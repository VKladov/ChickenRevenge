using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanMoveToGateState : PapuanState
{
    public PapuanMoveToGateState(Papuan papuan) : base(papuan) { }

    public override void Enter()
    {
        Papuan.Animator.SetFloat("walkSpeed", Papuan.Speed);
        Papuan.TargetSlote = Papuan.TargetGate.GetSlote();
        Papuan.SetTargetEnemy(null);
    }

    public override void Exit() { }

    public override void Update()
    {
        if (Papuan.TargetSlote == null)
        {
            Papuan.Move((Papuan.TargetSlote.transform.position - Papuan.transform.position).normalized);
        }
        else
        {
            Papuan.Move((Papuan.TargetGate.transform.position - Papuan.transform.position).normalized);
        }
    }
}
