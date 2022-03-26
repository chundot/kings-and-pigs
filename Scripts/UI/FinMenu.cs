using System;
using Godot;

public class FinMenu : Control
{
    private SceneTrans _trans;
    public override void _Ready()
    {
        _trans = GetChild<SceneTrans>(1);
    }

    public void OnMenu()
    {
        _trans.OnInStop += () => GetTree().ChangeScene("res://Scene/MainMenu.tscn");
        _trans.TransIn();
    }
}
