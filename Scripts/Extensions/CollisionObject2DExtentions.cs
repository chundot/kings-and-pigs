using kingsandpigs.Scripts.Common;

namespace Godot
{
    public static class CollisionObject2DExtentions
    {
        public static bool GetLayerBit(this CollisionObject2D obj, LayerEnum layer)
        {
            return obj.GetCollisionLayerBit(layer.GetHashCode());
        }
        public static bool GetMaskBit(this CollisionObject2D obj, LayerEnum mask)
        {
            return obj.GetCollisionLayerBit(mask.GetHashCode());
        }
        public static void DisLayerBit(this CollisionObject2D obj, LayerEnum layer)
        {
            obj.SetCollisionLayerBit(layer.GetHashCode(), false);
        }
        public static void EnLayerBit(this CollisionObject2D obj, LayerEnum layer)
        {
            obj.SetCollisionLayerBit(layer.GetHashCode(), true);
        }
        public static void DisMaskBit(this CollisionObject2D obj, LayerEnum layer)
        {
            obj.SetCollisionMaskBit(layer.GetHashCode(), false);
        }
        public static void EnMaskBit(this CollisionObject2D obj, LayerEnum layer)
        {
            obj.SetCollisionMaskBit(layer.GetHashCode(), true);
        }
    }
}
