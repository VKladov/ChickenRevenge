using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanGateSloteDistanceTransition : PapuanTransition
{
    private float _distance;

    public PapuanGateSloteDistanceTransition(Papuan papuan, PapuanState papuanState, float distance) : base(papuan, papuanState)
    {
        _distance = distance;
    }

    public override bool NeedTransit()
    {
        return Vector3.Distance(Papuan.TargetSlote.transform.position, Papuan.transform.position) < _distance;
    }
}
