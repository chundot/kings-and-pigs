using System;
using Godot;

public class Diamond : RigidBody2D
{
    private Vector2 Velocity = Vector2.Zero;
    public State CurState = 0;
    private AnimatedSprite _sprite;
    public override void _Ready()
    {
        _sprite = GetChild<AnimatedSprite>(0);
    }

    public override void _PhysicsProcess(float delta)
    {
        SpeedHandler();
    }

    public void ToHit()
    {
        _sprite.Play("Hit");
    }
    private void SpeedHandler(float factor = 0.1f)
    {
        Velocity.x = Mathf.Lerp(Velocity.x, 0, factor);
    }

    public void OnAnimationFinished()
    {
        if (_sprite.Animation == "Hit") QueueFree();
    }

    public enum State
    {
        Idle = 0,
        Hit
    }
}
