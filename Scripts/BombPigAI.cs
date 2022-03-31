using System;
using Godot;
using kingsandpigs.Scripts.Common;

namespace kingsandpigs.Scripts
{
    public class BombPigAI : BaseAI<BombPig, BombPigAI.State>
    {
        [Export] public int Dir = -1;
        private bool _trigger;
        private readonly Random _rnd = new();
        private Area2D _target;
        private float _aimTimer = 1f;
        public override void _Ready()
        {
            CurState = State.Idle;
            base._Ready();
        }

        protected override void StateUpdate(float delta)
        {
            var state = CurState switch
            {
                State.Idle => Idle(delta),
                State.Wander => Wander(delta),
                State.Attack => Attack(delta),
                _ => CurState
            };
            if (NextState != default)
            {
                CurState = NextState;
                NextState = default;
            }
            else
                CurState = state;
        }

        private State Idle(float delta)
        {
            if (TransTimer < 0 && _trigger)
            {
                TransTimer = _rnd.Next(2, 4);
                return State.Wander;
            }
            else
                TransTimer -= delta;
            return CurState;
        }
        private State Wander(float delta)
        {
            if (TransTimer < 0)
            {
                TransTimer = _rnd.Next(2, 5);
                return State.Idle;
            }
            TransTimer -= delta;
            if (Body.IsOnWall() || (Body.IsOnFloor() && !RayCast.IsColliding())) Dir = -Dir;
            Body.MovementHandler(Dir);
            return CurState;
        }

        private State Attack(float delta)
        {
            var dir = _target.GlobalPosition.x > GlobalPosition.x ? 1 : -1;
            Body.MovementHandler(dir, 0);
            if (Body.CurState is not BombPig.State.Throwing)
            {
                if (_aimTimer < 0)
                {
                    Body.NextState = BombPig.State.Throwing;
                    Body.Dir = (_target.GlobalPosition - GlobalPosition).Normalized();
                }
                else _aimTimer -= delta;
            }
            return CurState;
        }

        public void OnViewRangeEntered(Area2D area)
        {
            _trigger = true;
            _target = area;
            Body.Dlg.Display(DlgType.ExcIn);
            NextState = State.Attack;
        }
        public void OnViewRangeExited(Area2D area)
        {
            if (_aimTimer < .6f) return;
            Body.Dlg.Display(DlgType.ItgIn);
            NextState = State.Idle;
        }

        public enum State
        {
            NoState = 0,
            Idle,
            Wander,
            Attack
        }
    }
}