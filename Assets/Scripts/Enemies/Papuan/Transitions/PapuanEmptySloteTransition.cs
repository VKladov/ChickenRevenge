using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanEmptySloteTransition : PapuanTransition
{
    public PapuanEmptySloteTransition(Papuan papuan, PapuanState targetState) : base(papuan, targetState) { }

    public override bool NeedTransit()
    {
        return Papuan.TargetSlote != null && Papuan.TargetSlote.IsEmpty;
    }
}
