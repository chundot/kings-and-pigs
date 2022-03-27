using Godot;

public class Heart : RigidBody2D
{
    public bool Hit = false;
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
        Hit = true;
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
}
