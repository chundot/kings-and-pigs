using Godot;
using kingsandpigs.Scripts.Common;
using kingsandpigs.Scripts.UI;

public class PlayerController : Node2D
{
    private BaseBody _body;
    public override void _Ready()
    {
        _body = GetChild<BaseBody>(0);
        var hud = GetNode<LevelHUDManager>("../LevelHUD");
        _body.OnHealthChange += hud.HealthChange;
        _body.OnDiamondChanged += hud.DiamondChange;
    }

}
