using Godot;
using kingsandpigs.Scripts.UI;

public class LevelManager : Node2D
{
    private LevelHUDManager _hud;
    private PlayerController _player;
    [Export] public string NextLevel;
    public override void _Ready()
    {
        _hud = GetNode<LevelHUDManager>("LevelHUD");
        _player = GetNode<PlayerController>("Player");
        _player.UpdateEvent(_hud.HealthChange, _hud.DiamondChange, GoNextLevel);
    }

    public void GoNextLevel()
    {
        GetTree().ChangeScene($"res://Scene/Levels/{NextLevel}.tscn");
    }
}
