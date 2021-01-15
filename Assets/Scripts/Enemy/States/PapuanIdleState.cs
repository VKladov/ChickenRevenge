using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanIdleState : PapuanState
{
    public PapuanIdleState(Papuan papuan) : base(papuan) { }

    public override void Enter()
    {
        Papuan.Animator.SetFloat("walkSpeed", 0f);
    }

    public override void Exit() { }

    public override void Update() { }
}
