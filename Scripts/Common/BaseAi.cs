using System;
using Godot;

namespace kingsandpigs.Scripts.Common
{
    public class BaseAI<TBody, TState> : Node2D where TBody : BaseBody where TState : Enum
    {
        protected RayCast2D RayCast;
        protected TBody Body;
        protected TState CurState = default;
        protected TState NextState;
        protected bool IsDead = false;
        protected float TransTimer = 1f;
        protected bool Triggered;
        public override void _Ready()
        {
            Body = GetParent<Position2D>().GetParent<TBody>();
            Triggered = Body.Triggered;
            Body.OnDeath += () =>
            {
                QueueFree();
                IsDead = true;
            };
            RayCast = GetChild<RayCast2D>(0);
        }
        public override void _PhysicsProcess(float delta)
        {
            StateUpdate(delta);
            TransTimer = TransTimer < 0 ? TransTimer : TransTimer - delta;
        }

        protected virtual void StateUpdate(float delta) { }
    }

}