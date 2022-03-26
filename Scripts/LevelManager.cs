using Godot;
using kingsandpigs.Scripts.Common;
using kingsandpigs.Scripts.UI;

public class LevelManager : Node2D
{
    private LevelHUDManager _hud;
    private KingController _player;
    [Export] public string NextLevel;
    public override void _Ready()
    {
        _hud = GD.Load<PackedScene>("res://Nodes/HUD/LevelHUD.tscn").Instance<LevelHUDManager>();
        AddChild(_hud);
        _hud.OnNextLevel += GoNextLevel;
        _player = GetNode<KingController>("Player");
        _player.UpdateEvent(_hud.HealthChange, _hud.DiamondChange, _hud.TransIn);
    }


    public void GoNextLevel()
    {
        GlobalVar.Diamond = _hud.DiamondNum;
        if (NextLevel.Contains("Level"))
            GetTree().ChangeScene($"res://Scene/Levels/{NextLevel}.tscn");
        else GetTree().ChangeScene($"res://Scene/{NextLevel}.tscn");
    }
}
