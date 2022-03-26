using System;
using Godot;

public class Door : AnimatedSprite
{
    [Export] public bool IsEntry;
    public string NextLevel;
    public bool IsOpen;
    private KingController _player;
    private Sprite _tip;
    public override void _Ready()
    {
        _tip = GetChild<Sprite>(0);
        if (IsEntry) Play("Opening");
    }

    public void OnAnimationFinished()
    {
        if (Animation == "Closing")
            Play("Idle");
        else if (IsEntry && Animation == "Opening")
            Play("Closing");
        else if (!IsEntry && Animation == "Opening")
        {
            if (_player != null) _player.CanEnterDoor = true;
            IsOpen = true;
            _tip.Visible = true;
        }
    }

    public void OnPlayerEnter(Area2D area)
    {
        if (IsEntry) return;
        if (_player is null) _player = area.GetParent().GetParent<KingController>();
        Play("Opening");
    }
    public void OnPlayerExit(Area2D area)
    {
        if (IsEntry) return;
        if (_player is null) _player = area.GetParent().GetParent<KingController>();
        _player.CanEnterDoor = false;
        Play("Closing");
        _tip.Visible = false;
    }
}
