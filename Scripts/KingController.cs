using System;
using Godot;
using kingsandpigs.Scripts.Common;

public class KingController : PlayerController<King>
{

    public override void _Ready()
    {
        base._Ready();
        Body.OnDeath += () => GetNode<Node2D>("King/Death").Visible = true;
        if (Entering)
            Body.NextState = King.State.DoorOut;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if (CanEnterDoor && Input.IsActionJustPressed("enter_door"))
            Body.NextState = King.State.DoorIn;
        // Movement
        var dir = Input.GetActionStrength("right") - Input.GetActionStrength("left");
        Body.MovementHandler(dir);
        // Jump
        if (Input.IsActionJustPressed("jump")) JumpAction = .06f;
        if (Input.IsActionJustReleased("jump")) Body.GravityTimer = -.1f;
        if (JumpAction > 0)
        {
            if (Body.JumpHandler()) JumpAction = 0;
            JumpAction -= delta;
        }
        if (Input.IsActionJustPressed("suicide")) Body.HealthChange(1);
    }
    public void UpdateEvent(Action<int, int> onHealthChange, Action<int> onDiamondChange, Action onNextLevel)
    {
        Body.OnHealthChange += onHealthChange;
        Body.OnDiamondChanged += onDiamondChange;
        Body.OnNextLevel += onNextLevel;
    }
}