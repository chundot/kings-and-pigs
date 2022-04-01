using Godot;
using kingsandpigs.Scripts.Common;

public class SelectMenu : Control
{
    private SceneTrans _trans;
    private SfxMgr _sfx;

    public override void _Ready()
    {
        GetTree().Paused = false;
        var grid = GetChild<Panel>(0).GetChild<GridContainer>(1);
        _trans = GetChild<SceneTrans>(1);
        _sfx = GetChild<SfxMgr>(2);
        var lvlBtn = GD.Load<PackedScene>("res://Nodes/HUD/LevelButton.tscn");
        for (int i = 1; i <= GlobalVar.LevelNum; i++)
        {
            var btn = lvlBtn.Instance<LevelBtn>();
            btn.Level = i;
            btn.OnBtnClick += GoToLevel;
            grid.AddChildDefered(btn);
        }
    }

    public void GoToLevel(int level)
    {
        GlobalVar.CurLevel = level;
        SaveLoad.Save();
        _trans.OnInStop += () => GetTree().ChangeScene($"res://Scene/Levels/Level{level}-1.tscn");
        _trans.TransIn();
    }

    public void OnBack()
    {
        _trans.OnInStop += () => GetTree().ChangeScene("res://Scene/MainMenu.tscn");
        _trans.TransIn();
    }

    public void OnHover()
    {
        _sfx.Play("Hover");
    }
}
