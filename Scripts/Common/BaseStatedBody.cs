using System;
using Godot;

namespace kingsandpigs.Scripts.Common
{
    public class BaseStatedBody<TEnum> : BaseBody where TEnum : Enum
    {
        protected TEnum CurState;
        protected TEnum NextState = default;

        protected virtual TEnum TransTo(TEnum nextState)
        {
            DebugInfo(nextState);
            BeforeTrans(nextState);
            CurState = nextState;
            PostTrans();
            return CurState;
        }

        protected virtual void BeforeTrans(TEnum nextState)
        {
        }

        protected virtual void PostTrans()
        {
            Player.Play(CurState.ToString());
        }

        private void DebugInfo(TEnum nextState)
        {
            Info.GetChild<Label>(0).Text = nextState.ToString();
        }

        #region SLOTS

        public virtual void OnAttackBoxEnter(Node2D body)
        {
            if (body is TileMap tileMap)
            {
                GD.Print(body.GetType().Name);
            }
        }

        public virtual void OnHitBoxEnter(Area2D area)
        {
            var isDmgFromAttack = area.GetLayerBit(LayerEnum.AttackBox);
            if (isDmgFromAttack && InvincibleTimer <= 0)
            {
                InvincibleTimer = 0.3f;
                HealthChange(1);
                if (health == 0)
                {
                    GD.Print(area.GetParent<Position2D>().GetParent<KinematicBody2D>().Name);
                }
            }
        }

        #endregion
    }
}
