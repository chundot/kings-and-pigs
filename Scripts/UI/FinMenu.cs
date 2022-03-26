using System;
using Godot;
using kingsandpigs.Scripts.Common;

public class FinMenu : Control
{
    private SceneTrans _trans;
    public override void _Ready()
    {
        if (GlobalVar.CurLevel == GlobalVar.LevelNum) GetChild<Panel>(0).GetChild<Button>(1).Disabled = true;
        _trans = GetChild<SceneTrans>(1);
        _trans.OnInStop += () => GlobalVar.Diamond = 0;
    }

    public void OnMenu()
    {
        _trans.OnInStop += () => GetTree().ChangeScene("res://Scene/MainMenu.tscn");
        _trans.TransIn();
    }
    public void OnNext()
    {
        GlobalVar.UnlockedLevel = Mathf.Max(++GlobalVar.CurLevel, GlobalVar.UnlockedLevel);
        _trans.OnInStop += () => GetTree().ChangeScene($"res://Scene/Levels/Level{GlobalVar.CurLevel}-1.tscn");
        _trans.TransIn();
    }
}
