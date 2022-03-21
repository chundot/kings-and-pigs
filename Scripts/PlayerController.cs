using System;
using Godot;
using kingsandpigs.Scripts.Common;
using kingsandpigs.Scripts.UI;

public class PlayerController : Node2D
{
    private BaseBody _body;
    public override void _Ready()
    {
        _body = GetChild<BaseBody>(0);
    }
    public void UpdateEvent(Action<int, int> onHealthChange, Action<int> onDiamondChange)
    {
        _body.OnHealthChange += onHealthChange;
        _body.OnDiamondChanged += onDiamondChange;
    }
}
