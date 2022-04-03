using Godot;

public class Scenes
{
    public static PackedScene Pig = GD.Load<PackedScene>("res://Nodes/Prefabs/PigE.tscn");
    public static PackedScene BombPig = GD.Load<PackedScene>("res://Nodes/Prefabs/BombPigE.tscn");
    public static PackedScene Bomb = GD.Load<PackedScene>("res://Nodes/Bomb.tscn");
    public static PackedScene Heart = GD.Load<PackedScene>("res://Nodes/Heart.tscn");
    public static PackedScene LevelHUD = GD.Load<PackedScene>("res://Nodes/HUD/LevelHUD.tscn");
    public static PackedScene Diamond = GD.Load<PackedScene>("res://Nodes/Diamond.tscn");
    public static PackedScene FlyingCrate = GD.Load<PackedScene>("res://Nodes/Crate/FlyingCrate.tscn");
    public static PackedScene Crate = GD.Load<PackedScene>("res://Nodes/Crate/Crate.tscn");
    public static PackedScene CratePig = GD.Load<PackedScene>("res://Nodes/Prefabs/CratePigE.tscn");
    public static PackedScene Frag = GD.Load<PackedScene>("res://Nodes/Crate/CrateFrag.tscn");
}