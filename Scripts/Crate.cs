using System;
using Godot;
using kingsandpigs.Scripts.Common;

public class Crate : RigidBody2D
{
    private AnimatedSprite _sprite;
    private AudioStreamPlayer _audio;
    private int dir = 0;
    [Export] public bool FaceRight = false;
    public override void _Ready()
    {
        _sprite = GetChild<AnimatedSprite>(0);
        _audio = GetChild<AudioStreamPlayer>(1);
        _sprite.FlipH = FaceRight;
    }

    public void ToHit()
    {
        GlobalEvent.CameraShake?.Invoke(.08f, 4);
        _sprite.Play("Hit");
        _audio.Play();
    }

    private void GenerateFragments()
    {
        var rnd = new Random();
        var parent = GetParent();
        for (var i = 0; i < 4; i++)
        {
            var node = Scenes.Frag.Instance<CrateFrag>();
            node.GlobalPosition = GlobalPosition;
            node.Set(i, Vector2.Right * dir * rnd.Next(150, 240) + Vector2.Up * rnd.Next(80, 150));
            parent.AddChildDefered(node);
        }
    }

    private void GenerateRndItems()
    {
        var parent = GetParent();
        var rnd = new Random();
        var num = rnd.Next(0, 3);
        for (int i = 0; i < num; i++)
        {
            var node = Scenes.Diamond.Instance<Diamond>();
            node.GlobalPosition = GlobalPosition;
            node.Impulse = Vector2.Right * dir * rnd.Next(-40, 120) + Vector2.Up * rnd.Next(40, 90);
            parent.AddChildDefered(node);
        }
        num = rnd.Next(0, 2);
        if (num != 0)
        {
            var node = Scenes.Heart.Instance<Heart>();
            node.GlobalPosition = GlobalPosition;
            node.Impulse = Vector2.Right * dir * rnd.Next(-40, 120) + Vector2.Up * rnd.Next(40, 80);
            parent.AddChildDefered(node);
        }
    }

    public void OnAnimationFinished()
    {
        if (_sprite.Animation == "Hit")
        {
            GenerateFragments();
            GenerateRndItems();
            _sprite.Visible = false;
            GetChild<CollisionShape2D>(3).Disabled = true;
        }
    }

    public void AudioFinished()
    {
        QueueFree();
    }

    public void OnHitBoxEntered(Area2D body)
    {
        if (_sprite.Animation == "Hit") return;
        var x = GlobalPosition.x - body.GlobalPosition.x;
        dir = x > 0 ? 1 : -1;
        ToHit();
    }
}
