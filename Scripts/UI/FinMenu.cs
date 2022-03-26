using System;
using Godot;
using kingsandpigs.Scripts.Common;

public class FinMenu : Control
{
    private SceneTrans _trans;
    public override void _Ready()
    {
        _trans = GetChild<SceneTrans>(1);
        _trans.OnInStop += () => GlobalVar.Diamond = 0;
    }

    public void OnMenu()
    {
        _trans.OnInStop += () => GetTree().ChangeScene("res://Scene/MainMenu.tscn");
        _trans.TransIn();
    }
}
