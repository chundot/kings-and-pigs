using kingsandpigs.Scripts.Common;

namespace Godot
{
    public static class CollisionObject2DExtentions
    {
        public static bool GetLayerBit(this CollisionObject2D obj, LayerEnum layer)
        {
            return obj.GetCollisionLayerBit(layer.GetHashCode());
        }

        public static bool SetLayerBit(this CollisionObject2D obj, LayerEnum layer)
        {
            var tmp = !obj.GetLayerBit(layer);
            obj.SetCollisionLayerBit(layer.GetHashCode(), tmp);
            return tmp;
        }
    }
}
