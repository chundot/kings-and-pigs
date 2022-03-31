using System;
using Godot;
using kingsandpigs.Scripts.Common;

namespace kingsandpigs.Scripts
{
    public class BombPig : BaseStatedBody<BombPig.State>
    {
        public bool Hit;
        public Vector2 Dir;
        public override void _Ready()
        {
            base._Ready();
            TransTo(State.Picking);
        }

        protected override void StateUpdate(float delta)
        {
            var state = CurState switch
            {
                State.Idle => Idle(),
                State.Run => Run(),
                _ => Picking(),
            };
            if (NextState != default)
            {
                state = NextState;
                NextState = default;
            }
            if (state != CurState) TransTo(state);
        }

        public State Idle()
        {
            SpeedHandler();
            if (Mathf.Abs(Velocity.x) > 4) return State.Run;
            return CurState;
        }

        public State Run()
        {
            SpeedHandler();
            if (Mathf.Abs(Velocity.x) < 4) return State.Idle;
            return CurState;
        }

        public State Picking()
        {
            SpeedHandler();
            return CurState;
        }

        public void MovementHandler(int dir, float factor = 1)
        {
            if (CurState is State.Picking or State.Throwing) return;
            SpriteAnchor.Scale = new Vector2(-dir, 1);
            Velocity.x = Mathf.Lerp(Velocity.x, dir * Speed * factor, 0.2f);
        }

        public void ThrowBomb()
        {
            Hit = true;
            var parent = GetParent();
            var bombI = Scenes.Bomb.Instance<Bomb>();
            bombI.GlobalPosition = GlobalPosition;
            parent.CallDeferred("add_child", bombI);
            var factor = Dir.y < .1f ? .2f : .8f;
            bombI.CallDeferred("apply_central_impulse", Dir * bombI.Weight * 1.5f + Vector2.Up * bombI.Weight * factor);
            bombI.BombOn = true;
        }

        public void OnThrowed()
        {
            var parent = GetParent();
            var pigI = Scenes.Pig.Instance<Pig>();
            pigI.GlobalPosition = GlobalPosition;
            parent.CallDeferred("add_child", pigI);
            QueueFree();
        }

        public void OnAnimationFinished(string name)
        {
            _ = Enum.TryParse(name, out State state);
            if (state is State.Picking)
                NextState = State.Idle;
            else if (state is State.Throwing)
                OnThrowed();
        }
        public override void OnHitBoxEnter(Area2D area)
        {
            var isDmgFromAttack = area.GetLayerBit(LayerEnum.AttackBox);
            if (isDmgFromAttack && InvincibleTimer <= 0 && !Hit)
            {
                Hit = true;
                var pigI = Scenes.Pig.Instance<Pig>();
                var bombI = Scenes.Bomb.Instance<Bomb>();
                var parent = GetParent();
                pigI.GlobalPosition = GlobalPosition;
                bombI.GlobalPosition = GlobalPosition;
                pigI.Dmged = true;
                parent.CallDeferred("add_child", pigI);
                parent.CallDeferred("add_child", bombI);
                QueueFree();
            }
        }

        public enum State
        {
            NoState = 0,
            Idle,
            Picking,
            Run,
            Throwing
        }
    }
}