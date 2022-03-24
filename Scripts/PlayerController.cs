using System;
using Godot;
using kingsandpigs.Scripts.Common;
using kingsandpigs.Scripts.UI;

public class PlayerController : Node2D
{
    private BaseBody _body;
    [Export] public bool Entering;
    public bool CanEnterDoor;
    public override void _Ready()
    {
        _body = GetChild<BaseBody>(0);
        if (Entering && _body is King king)
            king.NextState = King.State.DoorOut;
    }
    public override void _PhysicsProcess(float delta)
    {
        if (CanEnterDoor && Input.IsActionJustPressed("enter_door") && _body is King king)
            king.NextState = King.State.DoorIn;
    }
    public void UpdateEvent(Action<int, int> onHealthChange, Action<int> onDiamondChange, Action onNextLevel)
    {
        _body.OnHealthChange += onHealthChange;
        _body.OnDiamondChanged += onDiamondChange;
        if (_body is King king)
            king.OnNextLevel += onNextLevel;
    }

}
