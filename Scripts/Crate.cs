using System;
using Godot;

public class Crate : RigidBody2D
{
    private AnimatedSprite _sprite;
    private Vector2 _velocity = Vector2.Zero;
    private int dir = 0;
    [Export] public bool FaceRight = false;
    public override void _Ready()
    {
        _sprite = GetChild<AnimatedSprite>(0);
        _sprite.FlipH = FaceRight;
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
            var node = frag.Instance<CrateFrag>();
            node.GlobalPosition = GlobalPosition;
            node.Set(i, Vector2.Right * dir * rnd.Next(150, 240) + Vector2.Up * rnd.Next(80, 150));
            parent.AddChild(node);
        }
    }

    private void GenerateRndItems()
    {
        var parent = GetParent();
        var rnd = new Random();
        var num = rnd.Next(0, 3);
        var diamond = GD.Load<PackedScene>("res://Nodes/Diamond.tscn");
        for (int i = 0; i < num; i++)
        {
            var node = diamond.Instance<Diamond>();
            node.GlobalPosition = GlobalPosition;
            node.Impulse = Vector2.Right * dir * rnd.Next(-40, 120) + Vector2.Up * rnd.Next(40, 90);
            parent.AddChild(node);
        }
        num = rnd.Next(0, 2);
        if (num != 0)
        {
            var heart = GD.Load<PackedScene>("res://Nodes/Heart.tscn");
            var node = heart.Instance<Heart>();
            node.GlobalPosition = GlobalPosition;
            node.Impulse = Vector2.Right * dir * rnd.Next(-40, 120) + Vector2.Up * rnd.Next(40, 80);
            parent.AddChild(node);
        }
    }

    public void OnAnimationFinished()
    {
        if (_sprite.Animation == "Hit")
        {
            GenerateFragments();
            GenerateRndItems();
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
