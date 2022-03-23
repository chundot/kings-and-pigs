using System;
using Godot;

public class Diamond : RigidBody2D
{
    private Vector2 Velocity = Vector2.Zero;
    public State CurState = 0;
    private AnimatedSprite _sprite;
    public Vector2 Impulse;
    public override void _Ready()
    {
        _sprite = GetChild<AnimatedSprite>(0);
        ApplyCentralImpulse(Impulse);
    }

    public void ToHit()
    {
        _sprite.Play("Hit");
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
