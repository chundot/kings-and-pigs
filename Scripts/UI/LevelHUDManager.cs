using System.Collections.Generic;
using Godot;

namespace kingsandpigs.Scripts.UI;

public class LevelHUDManager : CanvasLayer
{
    private TextureRect _liveBar;
    private List<SmallHeart> _hearts = new();

    public override void _Ready()
    {
        InitHeart();
        _liveBar = GetChild<TextureRect>(0);
        _hearts.ForEach(h => _liveBar.AddChild(h));
    }


    private void InitHeart()
    {
        var smallHeart = GD.Load<PackedScene>("res://Nodes/HUD/SmallHeart.tscn");
        for (var i = 0; i < 3; i++)
        {
            var heart = smallHeart.Instance<SmallHeart>();
            heart.Position = new Vector2(i * 11 + 11, 10);
            _hearts.Add(heart);
        }
    }

    public void HealthChange(int newVal, int oldVal)
    {
        if (newVal < oldVal) _hearts[newVal].Hit();
        else _hearts[oldVal].Recover();
    }
}