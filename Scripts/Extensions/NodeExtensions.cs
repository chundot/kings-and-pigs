namespace Godot
{
    public static class NodeExtensions
    {
        public static void AddChildDefered(this Node node, Node child)
        {
            node.CallDeferred("add_child", child);
        }
    }
}