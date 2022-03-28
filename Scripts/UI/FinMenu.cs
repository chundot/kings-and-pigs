using System;
using Godot;
using kingsandpigs.Scripts.Common;

public class FinMenu : Control
{
    private SceneTrans _trans;
    private SfxMgr _sfx;
    private bool _pressed;
    public override void _Ready()
    {
        if (GlobalVar.CurLevel == GlobalVar.LevelNum) GetChild<Panel>(0).GetChild<Button>(1).Disabled = true;
        if (GlobalVar.CurLevel + 1 <= GlobalVar.LevelNum)
            GlobalVar.UnlockedLevel = Mathf.Max(GlobalVar.CurLevel + 1, GlobalVar.UnlockedLevel);
        _trans = GetChild<SceneTrans>(1);
        _sfx = GetChild<SfxMgr>(2);
        _trans.OnInStop += () => GlobalVar.Diamond = 0;
    }

    public void OnMenu()
    {
        if (_pressed) return;
        _pressed = true;
        _sfx.Play("Click");
        _trans.OnInStop += () => GetTree().ChangeScene("res://Scene/MainMenu.tscn");
        _trans.TransIn();
    }
    public void OnNext()
    {
        if (_pressed) return;
        _pressed = true;
        _sfx.Play("Click");
        GlobalVar.CurLevel += 1;
        _trans.OnInStop += () => GetTree().ChangeScene($"res://Scene/Levels/Level{GlobalVar.CurLevel}-1.tscn");
        _trans.TransIn();
    }
    public void OnHover()
    {
        _sfx.Play("Hover");
    }
}
