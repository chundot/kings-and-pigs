using System;
using Godot;

public class Crate : RigidBody2D
{
    private AnimatedSprite _sprite;
    private Vector2 _velocity = Vector2.Zero;
    private int dir = 0;
    public override void _Ready()
    {
        _sprite = GetChild<AnimatedSprite>(0);
    }

    public override void _PhysicsProcess(float delta)
    {

    }

    public void ToHit()
    {
        _sprite.Play("Hit");
    }

    private void GenerateFragments()
    {
        var rnd = new Random();
        var frag = GD.Load<PackedScene>("res://Nodes/Crate/CrateFrag.tscn");
        var parent = GetParent();
        for (var i = 0; i < 4; i++)
        {
            GD.Print(i);
            var node = frag.Instance<CrateFrag>();
            node.GlobalPosition = GlobalPosition;
            node.Set(i, Vector2.Right * dir * rnd.Next(10, 20));
            parent.AddChild(node);
            node.ApplyCentralImpulse(Vector2.Right * dir * rnd.Next(10, 20));
        }
    }

    private void GenerateRndItems()
    {
        var diamond = GD.Load<PackedScene>("res://Nodes/Diamond.tscn");
    }

    public void OnAnimationFinished()
    {
        if (_sprite.Animation == "Hit")
        {
            GenerateFragments();
            QueueFree();
        }
    }

    public void OnHitBoxEntered(Area2D body)
    {
        if (_sprite.Animation == "Hit") return;
        var x = GlobalPosition.x - body.GlobalPosition.x;
        dir = x > 0 ? 1 : -1;
        ToHit();
    }
}
