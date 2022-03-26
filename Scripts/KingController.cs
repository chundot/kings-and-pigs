using System;
using Godot;
using kingsandpigs.Scripts.Common;

public class KingController : PlayerController<King>
{

    public override void _Ready()
    {
        base._Ready();
        if (Entering)
            Body.NextState = King.State.DoorOut;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if (CanEnterDoor && Input.IsActionJustPressed("enter_door"))
            Body.NextState = King.State.DoorIn;
    }
    public void UpdateEvent(Action<int, int> onHealthChange, Action<int> onDiamondChange, Action onNextLevel)
    {
        Body.OnHealthChange += onHealthChange;
        Body.OnDiamondChanged += onDiamondChange;
        if (Body is King king)
            king.OnNextLevel += onNextLevel;
    }
}