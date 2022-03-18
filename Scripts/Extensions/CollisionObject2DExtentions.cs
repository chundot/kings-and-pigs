using Godot;
using kingsandpigs.Scripts.Common;

namespace kingsandpigs.Scripts.Extensions;

public static class CollisionObject2DExtentions
{
    public static bool GetLayerBit(this CollisionObject2D obj, LayerEnum layer)
    {
        return obj.GetCollisionLayerBit(layer.GetHashCode());
    }
}