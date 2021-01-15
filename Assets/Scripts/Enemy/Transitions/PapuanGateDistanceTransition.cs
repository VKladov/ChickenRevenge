using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanGateDistanceTransition : PapuanTransition
{
    private float _distance;
    private bool _more;

    public PapuanGateDistanceTransition(Papuan papuan, PapuanState targetState, float distance, bool more) : base(papuan, targetState)
    {
        _distance = distance;
        _more = more;
    }

    public override bool NeedTransit()
    {
        return Vector3.Distance(Papuan.transform.position, Papuan.TargetGate.transform.position) >= _distance == _more;
    }
}
