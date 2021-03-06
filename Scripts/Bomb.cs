using Godot;
using kingsandpigs.Scripts.Common;

public class Bomb : RigidBody2D
{
    public State CurState = State.Off;
    private State _nextState;
    private AnimationPlayer _player;
    private float _timer = .1f;
    private DirRayCasts _dir;
    public Vector2 Force;
    public bool BombOn;
    public override void _Ready()
    {
        _player = GetChild<AnimationPlayer>(1);
        _dir = GetChild<DirRayCasts>(5);
        if (BombOn) TransTo(State.On);
    }

    public override void _PhysicsProcess(float delta)
    {
        var state = CurState switch
        {
            State.Off => Off(),
            State.On => On(delta),
            State.Boom => Boom(),
            _ => CurState,
        };
        if (_nextState != 0)
        {
            state = _nextState;
            _nextState = 0;
        }
        if (CurState != state) TransTo(state);
    }

    private State Off()
    {
        return CurState;
    }

    private State On(float delta)
    {
        _timer -= delta;
        return CurState;
    }

    private State Boom()
    {
        GravityScale = 0;
        LinearVelocity = Vector2.Zero;
        return CurState;
    }

    private void TransTo(State state)
    {
        if (state is State.On) _dir.Enable();
        CurState = state;
        if (CurState is State.Boom)
            GlobalEvent.CameraShake?.Invoke(.15f, 8);
        _player.Play(state.ToString());
    }

    public void OnHitBoxBodyEntered(Node2D _)
    {
        if (CurState is State.On && _timer < 0)
        {
            GetChild<Sprite>(0).RotationDegrees = _dir.GetDir();
            _nextState = State.Boom;
        }
    }

    public void OnHitBoxEntered(Area2D area)
    {
        if (area.GetParent<Node2D>() is FlyingCrate) return;
        switch (CurState)
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
        _timer = .1f;
        var x = area.GlobalPosition.x;
        var dir = GlobalPosition.x < x ? -1 : 1;
        ApplyCentralImpulse((dir * Vector2.Right * 20 + Vector2.Up * 15) * Weight);
    }

    public void OnAnimationFinished(string name)
    {
        if (name is "Boom")
            QueueFree();
    }

    public enum State
    {
        NoState = 0,
        Off,
        On,
        Boom
    }
}
