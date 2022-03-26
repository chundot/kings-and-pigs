using System;
using Godot;

public class SceneTrans : ColorRect
{
    private AnimationPlayer _player;
    public event Action OnInStop, OnOutStop;
    public override void _Ready()
    {
        _player = GetChild<AnimationPlayer>(0);
        _player.Play("TransOut");
    }

    public void TransIn()
    {
        _player.Play("TransIn");
    }

    public void OnTransStop(string name)
    {
        if (name.Contains("In")) OnInStop?.Invoke();
        else if (name.Contains("Out")) OnOutStop?.Invoke();
    }
}
