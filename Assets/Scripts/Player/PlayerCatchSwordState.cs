using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;

    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;

        if (player.transform.position.x > sword.position.x && player.facingDirection == 1)
        {
            player.Flip();
        }
        else if (player.transform.position.x < sword.position.x && player.facingDirection == -1)
        {
            player.Flip();
        }

        AudioManager.instance.PlaySFX(31, null);

        player.fx.PlayDustFX();

        player.fx.ScreenShake(player.fx.shakeSwordImpact);

        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDirection, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.StopSFX(31);

        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
