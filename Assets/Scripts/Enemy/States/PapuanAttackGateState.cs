using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanAttackGateState : AttackState
{
    public PapuanAttackGateState(Papuan papuan, float attackDelay) : base(papuan, attackDelay) { }

    public override void Enter()
    {
        Papuan.Animator.SetTrigger("attack");

        _attacked = false;
        _timePassed = 0;
    }
}
