using Godot;
using System;
using kingsandpigs.Scripts;
using kingsandpigs.Scripts.Common;
using kingsandpigs.Scripts.Extensions;
using kingsandpigs.Scripts.UI;


public class King : BaseSprite<King.State>
{
    public override void _Ready()
    {
        base._Ready();
        TransTo(State.Idle);
        OnHealthChange += GetNode<LevelHUDManager>("../LevelHUD").HealthChange;
    }

    protected override void StateUpdate()
    {
        if (Input.IsActionJustPressed("suicide")) HealthChange(1);
        if (Input.IsActionJustPressed("recover")) HealthChange(-1);
        if (Input.IsActionJustPressed("sayhi")) Dlg.Display(DlgType.HiIn);
        var state = CurState switch
        {
            State.Idle => Idle(),
            State.Run => Run(),
            State.Jump => Jump(),
            State.Fall => Fall(),
            State.Ground => Ground(),
            State.Attack => Attack(),
            _ => CurState
        };

        if (NextState != default)
        {
            state = NextState;
            NextState = default;
        }

        if (state != CurState) TransTo(state);
    }

    private State Idle()
    {
        MovementHandler();
        if (JumpHandler()) return State.Jump;
        if (AttackHandler()) return State.Attack;
        if (Mathf.Abs(Velocity.x) > 12) return State.Run;
        if (IsOnFloor()) return State.Idle;
        if (Velocity.y < 0) return State.Jump;
        if (Velocity.y > 0) return State.Fall;
        return CurState;
    }

    private State Run()
    {
        MovementHandler();
        if (JumpHandler()) return State.Jump;
        if (AttackHandler()) return State.Attack;
        if (Mathf.Abs(Velocity.x) < 10) return State.Idle;
        if (IsOnFloor()) return State.Run;
        if (Velocity.y > 0) return State.Fall;
        return CurState;
    }

    private State Jump()
    {
        MovementHandler();
        if (AttackHandler()) return State.Attack;
        if (Velocity.y > 0) return State.Fall;
        return CurState;
    }

    private State Fall()
    {
        MovementHandler();
        if (AttackHandler()) return State.Attack;
        if (IsOnFloor()) return State.Ground;
        return CurState;
    }

    private State Ground()
    {
        if (JumpHandler(1.1f)) return State.Jump;
        SpeedHandler(0.1f);
        return CurState;
    }

    private State Attack()
    {
        SpeedHandler(0.1f);
        return CurState;
    }

    private void MovementHandler(float factor = 1f)
    {
        var dir = Input.GetActionStrength("right") - Input.GetActionStrength("left");
        if (dir != 0) SpriteAnchor.Scale = new Vector2(dir, 1);
        Velocity.x = dir * Speed * factor;
    }

    private bool JumpHandler(float factor = 1f)
    {
        if (!IsOnFloor()) return true;
        if (Input.IsActionJustPressed("jump"))
        {
            Velocity.y = -JumpForce * factor;
            return true;
        }

        return false;
    }

    private bool AttackHandler()
    {
        if (Input.IsActionJustPressed("attack"))
        {
            TransTo(State.Attack);
            Velocity += SpriteAnchor.Scale.x * Speed * 1.2f * Vector2.Right;
            return true;
        }

        return false;
    }

    public enum State
    {
        NoState = 0,
        Idle,
        Run,
        Jump,
        Fall,
        Attack,
        Hit,
        Dead,
        DoorIn,
        DoorOut,
        Ground
    }

    public void OnAnimationFinished(string name)
    {
        Enum.TryParse(name, out State state);
        NextState = state switch
        {
            State.Attack => !IsOnFloor() ? State.Fall : State.Idle,
            State.Ground => State.Idle,
            _ => NextState
        };
    }

    public override void OnHitBoxEnter(Area2D area)
    {
        var isDmgFromPig = area.GetLayerBit(LayerEnum.EnemyAttackBox);
        if (isDmgFromPig)
        {
            GD.Print("ouch!");
        }
    }
}