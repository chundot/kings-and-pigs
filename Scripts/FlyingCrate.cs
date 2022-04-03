using System;
using Godot;
using kingsandpigs.Scripts.Common;

public class FlyingCrate : RigidBody2D
{

    private AnimatedSprite _sprite;
    private AudioStreamPlayer _audio;
    public bool FaceRight = false;
    private float _timer = .2f;
    private int _dir = 0;
    public override void _Ready()
    {
        _sprite = GetChild<AnimatedSprite>(0);
        _audio = GetChild<AudioStreamPlayer>(1);
        _sprite.FlipH = FaceRight;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_timer > 0) _timer -= delta;
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
            node.Impulse = Vector2.Right * _dir * rnd.Next(-40, 120) + Vector2.Up * rnd.Next(40, 90);
            parent.AddChildDefered(node);
        }
    }

    private void GenerateFragments()
    {
        GenerateRndItems();
        var rnd = new Random();
        var parent = GetParent();
        for (var i = 0; i < 4; i++)
        {
            var node = Scenes.Frag.Instance<CrateFrag>();
            node.GlobalPosition = GlobalPosition;
            node.Set(i, Vector2.Right * _dir * rnd.Next(-40, 120) + Vector2.Up * rnd.Next(40, 90));
            parent.AddChildDefered(node);
        }
    }

    public void ToHit()
    {
        if (_timer > 0) return;
        GlobalEvent.CameraShake?.Invoke(.08f, 4);
        GetNode<CollisionShape2D>("HitBox/CollisionShape2D").SetDeferred("disabled", true);
        _sprite.Play("Hit");
        _audio.Play();
    }

    public void OnAnimationFinished()
    {
        if (_sprite.Animation == "Hit")
        {
            GenerateFragments();
            _sprite.Visible = false;
            GetChild<CollisionShape2D>(3).Disabled = true;
        }
    }

    public void OnHitBoxBodyEntered(Node2D body)
    {
        if (_sprite.Animation == "Hit") return;
        var x = GlobalPosition.x - body.GlobalPosition.x;
        _dir = x > 0 ? 1 : -1;
        ToHit();
    }

    public void AudioFinished()
    {
        QueueFree();
    }
}