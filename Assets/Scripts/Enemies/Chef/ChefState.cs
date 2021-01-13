using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChefState
{
    protected Chef _chef;
    protected Transition[] _transitions;

    public ChefState(Chef chef, Transition[] transitions)
    {
        _chef = chef;
        _transitions = transitions;
    }

    public abstract void HandleInput();
    public abstract void UpdateLogic();
    public abstract void UpdatePhisycs();
    public abstract void Enter();
    public abstract void Exit();
}
