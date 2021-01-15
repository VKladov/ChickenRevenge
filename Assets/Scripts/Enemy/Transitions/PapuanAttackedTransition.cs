using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanAttackedTransition : PapuanTransition
{
    public PapuanAttackedTransition(Papuan papuan, PapuanState targetState) : base(papuan, targetState) { }

    public override bool NeedTransit()
    {
        return Papuan.LastDamageSource != null && Vector3.Distance(Papuan.LastDamageSource.transform.position, Papuan.transform.position) < Papuan.ViewDistance;
    }
}
