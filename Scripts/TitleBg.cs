using System;
using Godot;

public class TitleBg : ParallaxBackground
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public override void _PhysicsProcess(float delta)
    {
        ScrollOffset += new Vector2(delta * 5, delta * 5);
    }
}
