using Godot;
using kingsandpigs.Scripts.Common;
using kingsandpigs.Scripts.UI;

public class LevelManager : Node2D
{
    private LevelHUDManager _hud;
    private PlayerController _player;
    [Export] public string NextLevel;
    public override void _Ready()
    {
        _hud = GetNode<LevelHUDManager>("LevelHUD");
        _hud.OnNextLevel += GoNextLevel;
        _player = GetNode<PlayerController>("Player");
        _player.UpdateEvent(_hud.HealthChange, _hud.DiamondChange, _hud.TransIn);
    }

    public void GoNextLevel()
    {
        GlobalVar.Diamond = _hud.DiamondNum;
        GetTree().ChangeScene($"res://Scene/Levels/{NextLevel}.tscn");
    }
}
