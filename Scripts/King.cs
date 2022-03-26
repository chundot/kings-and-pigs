using System;
using Godot;
using kingsandpigs.Scripts;
using kingsandpigs.Scripts.Common;


public class King : BaseStatedBody<King.State>
{
    public Action OnNextLevel;
    public override void _Ready()
    {
        base._Ready();
        TransTo(State.Idle);

        OnHealthChange += (newVal, oldVal) => NextState = newVal < oldVal ? State.Hit : NextState;
    }

    protected override void StateUpdate(float delta)
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
            State.Hit => Hit(),
            State.Dead => Dead(),
            State.DoorIn => DoorIn(),
            State.DoorOut => DoorOut(),
            _ => CurState
        };

        if (NextState != default)
        {
            state = NextState;
            NextState = default;
        }

        if (state != CurState) TransTo(state);
    }

    #region STATEFUNC
    private State Idle()
    {
        SpeedHandler();
        if (AttackHandler()) return State.Attack;
        if (Mathf.Abs(Velocity.x) > 12) return State.Run;
        if (IsOnFloor()) return State.Idle;
        if (Velocity.y > 0) return State.Fall;
        return CurState;
    }

    private State Run()
    {
        SpeedHandler();
        if (AttackHandler()) return State.Attack;
        if (Mathf.Abs(Velocity.x) < 10) return State.Idle;
        if (IsOnFloor()) return State.Run;
        if (Velocity.y > 0) return State.Fall;
        return CurState;
    }

    private State Jump()
    {
        SpeedHandler();
        if (IsOnFloor()) return State.Ground;
        if (AttackHandler()) return State.Attack;
        if (Velocity.y > 0) return State.Fall;
        return CurState;
    }

    private State Fall()
    {
        SpeedHandler();
        if (AttackHandler()) return State.Attack;
        if (IsOnFloor()) return State.Ground;
        return CurState;
    }

    private State Ground()
    {
        SpeedHandler(0.1f);
        return CurState;
    }

    private State Attack()
    {
        SpeedHandler(0.1f);
        return CurState;
    }

    private State Hit()
    {
        SpeedHandler(.05f);
        return CurState;
    }

    private State Dead()
    {
        SpeedHandler(.1f);
        return CurState;
    }
    private State DoorIn()
    {
        SpeedHandler();
        return CurState;
    }
    private State DoorOut()
    {
        SpeedHandler();
        return CurState;
    }
    #endregion

    public void MovementHandler(float dir)
    {
        var factor = 1f;
        if (CurState == State.Ground) factor = .6f;
        else if (CurState != State.Jump && CurState != State.Fall && CurState != State.Idle && CurState != State.Run) return;
        if (dir != 0) SpriteAnchor.Scale = new Vector2(-dir, 1);
        if (dir != 0) Velocity.x = dir * Speed * factor;
    }

    public bool JumpHandler()
    {
        if (CurState != State.Fall || FallTimer <= 0f)
            if (!IsOnFloor()) return false;
        var factor = 1f;
        NextState = State.Jump;
        Velocity.y = -JumpForce * factor;
        return true;
    }

    private bool AttackHandler()
    {
        if (Input.IsActionJustPressed("attack"))
        {
            TransTo(State.Attack);
            Velocity += -SpriteAnchor.Scale.x * Speed * 1.2f * Vector2.Right;
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
        _ = Enum.TryParse(name, out State state);
        NextState = state switch
        {
            State.Attack => !IsOnFloor() ? State.Fall : State.Idle,
            State.Ground => State.Idle,
            State.Hit => Health > 0 ? State.Idle : State.Dead,
            State.DoorOut => State.Idle,
            _ => NextState
        };
        if (state is State.DoorIn)
        {
            Visible = false;
            OnNextLevel?.Invoke();
        }
    }
}