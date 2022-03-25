using System;
using Godot;

public class Tip : AnimationPlayer
{
    public void OnAreaEntered(Area2D _)
    {
        Play("In");
    }
    public void OnAreaExited(Area2D _)
    {
        Play("Out");
    }
}
