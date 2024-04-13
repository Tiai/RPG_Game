using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherGroundedState : EnemyState
{
    protected Transform player;
    protected Enemy_Archer enemy;

    public ArcherGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.isPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.agroDiatance /*enemy can attack while player is behind him */)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
