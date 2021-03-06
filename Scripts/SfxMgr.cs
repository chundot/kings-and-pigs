using System;
using System.Linq;
using Godot;
using Godot.Collections;

public class SfxMgr : Node
{
    private readonly Dictionary<string, AudioStreamPlayer> _dic = new();
    public override void _Ready()
    {
        var children = GetChildren().Cast<AudioStreamPlayer>();
        foreach (var child in children)
            _dic[child.Name] = child;
    }

    public void Play(string name)
    {
        _dic[name]?.Play();
    }
}
