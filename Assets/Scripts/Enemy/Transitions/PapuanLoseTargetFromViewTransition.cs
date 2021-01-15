using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanLoseTargetFromViewTransition : PapuanTransition
{
    private float _delay;
    private float _timePassed = 0;

    public PapuanLoseTargetFromViewTransition(Papuan papuan, PapuanState targetState, float delay) : base(papuan, targetState)
    {
        _delay = delay;
    }

    public override bool NeedTransit()
    {
        _timePassed += Time.deltaTime;
        return Papuan.TargetEnemy == null ||
            (_timePassed >= _delay && Papuan.CouldSeeCharacter(Papuan.TargetEnemy) == false);
    }

    public override void Disable()
    {
        _timePassed = 0;
    }
}
