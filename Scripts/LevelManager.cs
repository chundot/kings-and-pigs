using Godot;
using kingsandpigs.Scripts.UI;

public class LevelManager : Node2D
{
    private LevelHUDManager _hud;
    private PlayerController _player;
    public override void _Ready()
    {
        _hud = GetNode<LevelHUDManager>("LevelHUD");
        _player = GetNode<PlayerController>("Player");
        _player.UpdateEvent(_hud.HealthChange, _hud.DiamondChange);
    }
}
