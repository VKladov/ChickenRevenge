using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanStopedAttackGateState : PapuanState
{
    public PapuanStopedAttackGateState(Papuan papuan) : base(papuan) { }

    public override void Enter()
    {
        if (Papuan.TargetSlote != null)
        {
            Papuan.TargetSlote.MakeEmpty();
            Papuan.TargetSlote = null;
        }
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
