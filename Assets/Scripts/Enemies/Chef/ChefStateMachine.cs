using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefStateMachine : MonoBehaviour
{
    public ChefState CurrentState;

    private void Awake()
    {
        
    }
}

public abstract class Transition
{
    public ChefState TargetState;
    public abstract bool NeedTransit();

    public Transition(ChefState targetState)
    {
        TargetState = targetState;
    }
}

public class BlockingState : ChefState
{
    public BlockingState(Chef chef, Transition[] transitions) : base(chef, transitions)
    {

    }

    public override void Enter()
    {
        _chef.TopCollider.GotShot += OnGotTopShot;
        _chef.BottomCollider.GotShot += OnGotBottomShot;
    }

    public override void Exit()
    {
        _chef.TopCollider.GotShot -= OnGotTopShot;
        _chef.BottomCollider.GotShot -= OnGotBottomShot;
    }

    public override void HandleInput() { }
    public override void UpdateLogic() { }
    public override void UpdatePhisycs() { }

    private void OnGotTopShot(Vector3 hitPoint, Vector3 direction)
    {
        _chef.SetAnimationTrigger(Chef.AnimatorTriggerBlockTop);
    }

    private void OnGotBottomShot(Vector3 hitPoint, Vector3 direction)
    {
        _chef.SetAnimationTrigger(Chef.AnimatorTriggerBlockBottom);
    }
}