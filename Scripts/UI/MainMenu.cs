using System;
using Godot;

public class MainMenu : Control
{
    private SceneTrans _trans;
    public override void _Ready()
    {
        _trans = GetChild<SceneTrans>(1);
    }

    public void OnStart()
    {
        _trans.OnInStop += () => GetTree().ChangeScene("res://Scene/Levels/Level1-1.tscn");
        _trans.TransIn();
    }
}
