using System;
using Godot;

public class Diamond : RigidBody2D
{
    public State CurState = 0;
    private AnimatedSprite _sprite;
    private AudioStreamPlayer _audio;
    public Vector2 Impulse;
    public override void _Ready()
    {
        _sprite = GetChild<AnimatedSprite>(0);
        _audio = GetChild<AudioStreamPlayer>(1);
        ApplyCentralImpulse(Impulse);
    }

    public void ToHit()
    {
        CurState = State.Hit;
        _sprite.Play("Hit");
        _audio.Play();
    }

    public void OnAnimationFinished()
    {
        if (_sprite.Animation == "Hit")
            _sprite.Visible = false;
    }

    public void OnAudioFinished()
    {
        QueueFree();
    }

    public enum State
    {
        Idle = 0,
        Hit
    }
}
