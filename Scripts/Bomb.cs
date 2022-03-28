using Godot;
using kingsandpigs.Scripts.Common;

public class Bomb : RigidBody2D
{
    private State _curState = State.Off;
    private State _nextState;
    private AnimationPlayer _player;
    private float _timer;
    private DirRayCasts _dir;
    public override void _Ready()
    {
        _player = GetChild<AnimationPlayer>(1);
        _dir = GetChild<DirRayCasts>(5);
    }

    public override void _PhysicsProcess(float delta)
    {
        var state = _curState switch
        {
            State.Off => Off(),
            State.On => On(delta),
            State.Boom => Boom(),
            _ => _curState,
        };
        if (_nextState != 0)
        {
            state = _nextState;
            _nextState = 0;
        }
        if (_curState != state) TransTo(state);
    }

    private State Off()
    {
        return _curState;
    }

    private State On(float delta)
    {
        _timer -= delta;
        return _curState;
    }

    private State Boom()
    {
        GravityScale = 0;
        LinearVelocity = Vector2.Zero;
        return _curState;
    }

    private void TransTo(State state)
    {
        if (state is State.On) _dir.Enable();
        _curState = state;
        _player.Play(state.ToString());
    }

    public void OnHitBoxBodyEntered(Node2D _)
    {
        if (_curState is State.On)
        {
            GetChild<Sprite>(0).RotationDegrees = _dir.GetDir();
            _nextState = State.Boom;
        }
    }

    public void OnHitBoxEntered(Area2D area)
    {
        switch (_curState)
        {
            case State.Boom:
                return;
            case State.Off:
                if (area.GetLayerBit(LayerEnum.AttackBox)) TransTo(State.On);
                else return;
                break;
            case State.On:
                if (area.GetLayerBit(LayerEnum.AttackBox) && _timer < 0) _nextState = State.Boom;
                return;
        }
        _timer = .15f;
        var x = area.GlobalPosition.x;
        var dir = GlobalPosition.x < x ? -1 : 1;
        ApplyCentralImpulse((dir * Vector2.Right * 20 + Vector2.Up * 15) * Weight);
    }

    public void OnAnimationFinished(string name)
    {
        if (name is "Boom")
            QueueFree();
    }

    enum State
    {
        NoState = 0,
        Off,
        On,
        Boom
    }
}
