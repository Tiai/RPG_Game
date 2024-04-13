using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter {  get; private set; }

    private float lastTimeAttacked;
    private float comboWindow = 2;


    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0;

        if(comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        AudioManager.instance.PlaySFX(comboCounter, null);

        player.animator.SetInteger("ComboCounter", comboCounter);

        float attackDirection = player.facingDirection;
        
        if (xInput != 0)
        {
            attackDirection = xInput;
        }

        player.setVelocity(player.attackMovement[comboCounter].x * attackDirection, player.attackMovement[comboCounter].y);


        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
        {
            player.setZeroVelocity();
        }


        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
