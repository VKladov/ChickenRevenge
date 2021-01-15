using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanLookForEmptySloteState : PapuanState
{
    public PapuanLookForEmptySloteState(Papuan papuan) : base(papuan) { }

    public override void Enter() { }

    public override void Exit()
    {

    }

    public override void Update()
    {
        Papuan.TargetSlote = Papuan.TargetGate.GetSlote();
    }
}
