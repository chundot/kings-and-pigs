using System;
using Godot;
using kingsandpigs.Scripts.Common;

public class PlayerController<TBody> : Node2D where TBody : BaseBody
{
    protected TBody Body;
    [Export] public bool Entering;
    public bool CanEnterDoor;
    public float JumpAction = -.1f;
    public override void _Ready()
    {
        Body = GetChild<TBody>(0);
    }
    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("restart")) GetTree().ReloadCurrentScene();
    }

}
