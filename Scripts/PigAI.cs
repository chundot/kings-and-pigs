using System;
using Godot;

namespace kingsandpigs.Scripts;

public class PigAI : Node
{
    private Pig _pig;
    public override void _Ready()
    {
        _pig = GetChild<Pig>(0);
    }
    
    enum State
    {
        Idle = 0,
        Wander,
        Follow,
        TryAttack
    }
}