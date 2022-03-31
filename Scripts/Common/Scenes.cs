using Godot;

public class Scenes
{
    public static PackedScene Pig = GD.Load<PackedScene>("res://Nodes/Prefabs/PigE.tscn");
    public static PackedScene Bomb = GD.Load<PackedScene>("res://Nodes/Bomb.tscn");
    public static PackedScene Heart = GD.Load<PackedScene>("res://Nodes/Heart.tscn");
    public static PackedScene LevelHUD = GD.Load<PackedScene>("res://Nodes/HUD/LevelHUD.tscn");
    public static PackedScene Diamond = GD.Load<PackedScene>("res://Nodes/Diamond.tscn");
    public static PackedScene Frag = GD.Load<PackedScene>("res://Nodes/Crate/CrateFrag.tscn");
}