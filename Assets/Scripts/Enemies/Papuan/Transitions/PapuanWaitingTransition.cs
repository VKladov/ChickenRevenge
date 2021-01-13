using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanWaitingTransition : PapuanTransition
{
    private float _delay;
    private float _timePassed = 0;

    public PapuanWaitingTransition(Papuan papuan, PapuanState targetState, float delay) : base(papuan, targetState)
    {
        _delay = delay;
    }

    public override bool NeedTransit()
    {
        _timePassed += Time.deltaTime;
        return _timePassed >= _delay;
    }

    public override void Disable()
    {
        _timePassed = 0;
    }
}
