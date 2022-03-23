using Godot;

public class Heart : RigidBody2D
{
    public bool Hit = false;
    private AnimatedSprite _sprite;
    public Vector2 Impulse;
    public override void _Ready()
    {
        _sprite = GetChild<AnimatedSprite>(0);
        ApplyCentralImpulse(Impulse);
    }

    public void ToHit()
    {
        Hit = true;
        _sprite.Play("Hit");
    }

    public void OnAnimationFinished()
    {
        if (_sprite.Animation == "Hit") QueueFree();
    }
}
