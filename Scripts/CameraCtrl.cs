using System;
using Godot;
using kingsandpigs.Scripts.Common;

public class CameraCtrl : Camera2D
{
    private Vector2 _curScreen = Vector2.Zero;
    private Vector2 _calcSize = new Vector2(480, 288);
    private Vector2 _scrnSize = new Vector2(512, 360);
    private BaseBody _player;
    public override void _Ready()
    {
        _player = GetNode<BaseBody>("../Player/King");
    }

    public override void _PhysicsProcess(float delta)
    {
        UpdateScreenByFixedSize();
    }

    // reference: https://gist.github.com/securas/2400b3fa1a31650a270618d1c8851ae6
    private void UpdateScreenByFixedSize()
    {
        var screen = (_player.GlobalPosition / _calcSize).Floor();
        if (screen.IsEqualApprox(_curScreen)) return;
        _curScreen = screen;
        GlobalPosition = _curScreen * _calcSize + 0.5f * _scrnSize;
    }
}
