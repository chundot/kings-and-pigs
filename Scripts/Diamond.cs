using System;
using Godot;

public class Diamond : KinematicBody2D
{
    private Vector2 Velocity = Vector2.Zero;
    private int MaxFallSpeed = 360;
    private int Gravity = 16;
    public State CurState = 0;
    private AnimatedSprite _sprite;
    public override void _Ready()
    {
        _sprite = GetChild<AnimatedSprite>(0);
    }

    public override void _PhysicsProcess(float delta)
    {
        Velocity = MoveAndSlide(Velocity, Vector2.Up);
        SpeedHandler();
        GravityHandler();
    }

    public void ToHit()
    {
        _sprite.Play("Hit");
    }
    private void SpeedHandler(float factor = 0.1f)
    {
        Velocity.x = Mathf.Lerp(Velocity.x, 0, factor);
    }

    private void GravityHandler()
    {
        Velocity.y = Mathf.Clamp(Velocity.y + Gravity, -MaxFallSpeed, MaxFallSpeed);
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
