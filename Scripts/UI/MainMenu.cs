using System;
using Godot;
using kingsandpigs.Scripts.Common;

public class MainMenu : Control
{
    private SceneTrans _trans;
    private SfxMgr _sfx;
    public override void _Ready()
    {
        SaveLoad.Load();
        GetTree().Paused = false;
        _trans = GetChild<SceneTrans>(1);
        _sfx = GetChild<SfxMgr>(2);
    }

    public void OnStart()
    {
        _sfx.Play("Click");
        _trans.OnInStop += () => GetTree().ChangeScene("res://Scene/SelectMenu.tscn");
        _trans.TransIn();
    }
    public void OnContinue()
    {
        _sfx.Play("Click");
        _trans.OnInStop += () => GetTree().ChangeScene($"res://Scene/Levels/Level{GlobalVar.CurLevel}-1.tscn");
        _trans.TransIn();
    }

    public void OnHover()
    {
        _sfx.Play("Hover");
    }
}
