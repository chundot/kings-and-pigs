using Godot;
using kingsandpigs.Scripts.Common;

public class SelectMenu : Control
{
    private SceneTrans _trans;
    public override void _Ready()
    {
        GetTree().Paused = false;
        var grid = GetChild<Panel>(0).GetChild<GridContainer>(1);
        _trans = GetChild<SceneTrans>(1);
        var lvlBtn = GD.Load<PackedScene>("res://Nodes/HUD/LevelButton.tscn");
        for (int i = 1; i <= GlobalVar.LevelNum; i++)
        {
            var btn = lvlBtn.Instance<LevelBtn>();
            btn.Level = i;
            btn.OnBtnClick += GoToLevel;
            grid.AddChild(btn);
        }
    }

    public void GoToLevel(int level)
    {
        GlobalVar.CurLevel = level;
        _trans.OnInStop += () => GetTree().ChangeScene($"res://Scene/Levels/Level{level}-1.tscn");
        _trans.TransIn();
    }
}
