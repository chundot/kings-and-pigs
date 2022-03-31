using System;
using Godot;
using kingsandpigs.Scripts.Common;

public class CameraCtrl : Camera2D
{
    private Vector2 _curScreen = Vector2.Zero;
    private Vector2 _calcSize = new(512, 320);
    private Vector2 _offset = new(308, 179);
    private BaseBody _player;
    private Random _rnd = new();
    private float _shakeTimer = -.1f;
    private float _shakeOffset = 2f;
    public override void _Ready()
    {
        _player = GetNode<BaseBody>("../Player/King");
        GlobalEvent.CameraShake = Shake;
        Position = _curScreen * _calcSize + _offset;
    }

    public override void _PhysicsProcess(float delta)
    {
        UpdateScreenByFixedSize();
        if (_shakeTimer > 0f)
        {
            RndOffset();
            _shakeTimer -= delta;
        }
        else Offset = Vector2.Zero;
    }

    // reference: https://gist.github.com/securas/2400b3fa1a31650a270618d1c8851ae6
    private void UpdateScreenByFixedSize()
    {
        var screen = (_player.GlobalPosition / _calcSize).Floor();
        if (screen.IsEqualApprox(_curScreen)) return;
        _curScreen = screen;
        Position = _curScreen * _calcSize + _offset;
    }

    public void Shake(float timer, float offset)
    {
        _shakeTimer = timer;
        _shakeOffset = offset;
    }

    public void RndOffset()
    {
        Offset = new Vector2((float)_rnd.NextDouble() * _shakeOffset - _shakeOffset / 2, (float)_rnd.NextDouble() * _shakeOffset - _shakeOffset / 2);
    }
}
