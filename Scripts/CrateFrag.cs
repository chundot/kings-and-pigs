using System;
using Godot;

public class CrateFrag : RigidBody2D
{
    public AnimatedSprite Sprite;
    private string _type = "0";
    private Vector2 _impulse;
    private float _timer = 3f;
    public override void _Ready()
    {
        Sprite = GetChild<AnimatedSprite>(0);
        Sprite.Play(_type);
        ApplyCentralImpulse(_impulse);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Modulate.a < .02f) QueueFree();
        if (_timer < 0f) Modulate -= new Color(0, 0, 0, delta);
        else
            _timer -= delta;
    }
    public void Set(int type, Vector2 impulse)
    {
        _type = type.ToString();
        _impulse = impulse;
    }
}
