using System;
using Godot;

public class CrateFrag : RigidBody2D
{
    public AnimatedSprite Sprite;
    private string _type = "0";
    private Vector2 _impulse;
    public override void _Ready()
    {
        Sprite = GetChild<AnimatedSprite>(0);
        Sprite.Play(_type);
        ApplyCentralImpulse(_impulse);
    }
    public void Set(int type, Vector2 impulse)
    {
        _type = type.ToString();
        _impulse = impulse;
    }
}
