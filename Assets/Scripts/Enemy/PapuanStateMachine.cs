using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Papuan))]
public class PapuanStateMachine : MonoBehaviour
{
    private string _currentStateName;
    private PapuanState _defaultState;
    public PapuanState _currentState;

    private void Awake()
    {
        Papuan papuan = GetComponent<Papuan>();

        PapuanState moveState = new PapuanMoveState(papuan);
        PapuanState searchingState = new PapuanIdleState(papuan);
        PapuanState standingNearPlayer = new PapuanLookAtPlayerState(papuan);
        PapuanState attackState = new AttackState(papuan, 0.3f);
        PapuanState attackBackState = new AttackBackState(papuan);
        PapuanState attackGateState = new PapuanAttackGateState(papuan, 0.3f);
        PapuanState moveToGate = new PapuanMoveToGateState(papuan);
        PapuanState moveToGateSlote = new PapuanMoveToGateSloteState(papuan);
        PapuanState standingNearGate = new PapuanLookForEmptySloteState(papuan);
        PapuanState standingInGateSlote = new PapuanStandingInGateSlote(papuan);
        PapuanState exitFromGateState = new PapuanStopedAttackGateState(papuan);

        PapuanTransition whenChooseTarget = new PapuanSeePlayerTransition(papuan, moveState);
        PapuanTransition whenCantSeeTarget = new PapuanLoseTargetFromViewTransition(papuan, moveToGate, 2);
        PapuanTransition whenTargetIsClose = new PapuanPlayerDistanceTransition(papuan, attackState, 0.5f, false);
        PapuanTransition whenTargetWentAway = new PapuanPlayerDistanceTransition(papuan, searchingState, 1.5f, true);
        PapuanTransition waitCooldownAfterAttack = new PapuanWaitingTransition(papuan, searchingState, 1);
        PapuanTransition whenSomeoneHit = new PapuanAttackedTransition(papuan, attackBackState);
        PapuanTransition waitAfterTurnToAttackBack = new PapuanWaitingTransition(papuan, moveState, 0.1f);
        PapuanTransition whenCloseToGate = new PapuanGateSloteDistanceTransition(papuan, standingNearGate, 3);
        PapuanTransition whenFoundEmptySlote = new PapuanEmptySloteTransition(papuan, moveToGateSlote);
        PapuanTransition whenMovedToGateSlote = new PapuanGateSloteDistanceTransition(papuan, standingInGateSlote, 0.2f);
        PapuanTransition whenStayInGateSlote = new PapuanGateSloteDistanceTransition(papuan, attackGateState, 0.2f);
        PapuanTransition waitCooldownAfterAttackGate = new PapuanWaitingTransition(papuan, standingInGateSlote, 1);
        PapuanTransition whenDistractedFromGate = new PapuanSeePlayerTransition(papuan, exitFromGateState);
        PapuanTransition waitDelayAfterDistraction = new PapuanWaitingTransition(papuan, moveState, 0.1f);

        searchingState.Transitions = new PapuanTransition[] { whenChooseTarget, whenSomeoneHit, whenCantSeeTarget };
        moveState.Transitions = new PapuanTransition[] { whenCantSeeTarget, whenTargetIsClose, whenSomeoneHit };
        standingNearPlayer.Transitions = new PapuanTransition[] { whenTargetWentAway, whenSomeoneHit };
        attackState.Transitions = new PapuanTransition[] { waitCooldownAfterAttack, whenSomeoneHit };
        attackBackState.Transitions = new PapuanTransition[] { waitAfterTurnToAttackBack };

        moveToGate.Transitions = new PapuanTransition[] { whenCloseToGate, whenChooseTarget };
        standingNearGate.Transitions = new PapuanTransition[] { whenFoundEmptySlote, whenChooseTarget };

        moveToGateSlote.Transitions = new PapuanTransition[] { whenMovedToGateSlote, whenChooseTarget, whenDistractedFromGate };
        standingInGateSlote.Transitions = new PapuanTransition[] { whenStayInGateSlote, whenDistractedFromGate };
        attackGateState.Transitions = new PapuanTransition[] { waitCooldownAfterAttackGate, whenDistractedFromGate };
        exitFromGateState.Transitions = new PapuanTransition[] { waitDelayAfterDistraction };

        _currentState = moveToGate;
    }

    private void Start()
    {
        SetState(_defaultState);
    }

    private void Update()
    {
        if (_currentState.NeedTransition(out PapuanState nextState))
            SetState(nextState);

        _currentState.Update();
    }

    private void SetState(PapuanState nextState)
    {
        if (nextState == null)
            return;

        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();

        _currentStateName = nextState.ToString();
    }
}
