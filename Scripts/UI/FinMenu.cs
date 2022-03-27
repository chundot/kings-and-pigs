using System;
using Godot;
using kingsandpigs.Scripts.Common;

public class FinMenu : Control
{
    private SceneTrans _trans;
    private SfxMgr _sfx;
    public override void _Ready()
    {
        if (GlobalVar.CurLevel == GlobalVar.LevelNum) GetChild<Panel>(0).GetChild<Button>(1).Disabled = true;
        _trans = GetChild<SceneTrans>(1);
        _sfx = GetChild<SfxMgr>(2);
        _trans.OnInStop += () => GlobalVar.Diamond = 0;
    }

    public void OnMenu()
    {
        _sfx.Play("Click");
        _trans.OnInStop += () => GetTree().ChangeScene("res://Scene/MainMenu.tscn");
        _trans.TransIn();
    }
    public void OnNext()
    {
        _sfx.Play("Click");
        GlobalVar.UnlockedLevel = Mathf.Max(++GlobalVar.CurLevel, GlobalVar.UnlockedLevel);
        _trans.OnInStop += () => GetTree().ChangeScene($"res://Scene/Levels/Level{GlobalVar.CurLevel}-1.tscn");
        _trans.TransIn();
    }
    public void OnHover()
    {
        _sfx.Play("Hover");
    }
}
