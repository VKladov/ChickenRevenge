using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PapuanTransition
{
    public PapuanState TargetState;
    protected Papuan Papuan;

    public PapuanTransition(Papuan papuan, PapuanState targetState)
    {
        Papuan = papuan;
        TargetState = targetState;
    }

    public abstract bool NeedTransit();

    public virtual void Disable() { }
}
