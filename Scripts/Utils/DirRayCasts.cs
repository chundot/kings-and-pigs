using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class DirRayCasts : Node2D
{
    private List<RayCast2D> _rays;
    public override void _Ready()
    {
        _rays = GetChildren().Cast<RayCast2D>().ToList();
    }

    public void Enable()
    {
        _rays.ForEach(r => r.Enabled = true);
    }

    public int GetDir()
    {
        // _rays.ForEach(r => r.Enabled = false);
        if (_rays[0].IsColliding()) return 180;
        else if (_rays[1].IsColliding()) return -90;
        else if (_rays[2].IsColliding()) return 90;
        return 0;
    }
}
