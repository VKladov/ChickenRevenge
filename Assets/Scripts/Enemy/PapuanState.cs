using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PapuanState
{
    protected Papuan Papuan;

    public PapuanTransition[] Transitions = new PapuanTransition[] { };

    public PapuanState(Papuan papuan)
    {
        Papuan = papuan;
    }

    public bool NeedTransition(out PapuanState nextState)
    {
        nextState = null;
        foreach (PapuanTransition transition in Transitions)
        {
            if (transition.NeedTransit())
            {
                nextState = transition.TargetState;
                DisableAllTransitions();
                return true;
            }
        }

        return false;
    }

    private void DisableAllTransitions()
    {
        foreach (PapuanTransition transition in Transitions)
            transition.Disable();
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void Update();
}
