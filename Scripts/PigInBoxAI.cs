using System;
using Godot;
using kingsandpigs.Scripts.Common;

namespace kingsandpigs.Scripts
{
    public class PigInBoxAI : BaseAI<PigInBox, PigInBoxAI.State>
    {
        private Area2D _target;
        private int _dir = 1;
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
                State.Attack => Attack(),
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

        private State Idle(float _)
        {
            return CurState;
        }
        private State Attack()
        {
            if (_target is null)
                return State.Idle;
            var xDelta = GlobalPosition.x - _target.GlobalPosition.x;
            _dir = xDelta > 0 ? -1 : 1;
            Body.ReadyJump(_dir);
            return CurState;
        }

        #region SLOTS
        public void OnAtkRangeEntered(Area2D _)
        {
            if (IsDead) return;
            Body.CanAttack = true;
            NextState = State.Attack;
        }

        public void OnAtkRangeExited(Area2D _)
        {
            if (IsDead) return;
            Body.CanAttack = false;
            NextState = State.Idle;
        }

        public void OnViewRangeEntered(Area2D area)
        {
            if (IsDead) return;
            Body.NextState = PigInBox.State.LookingOut;
            _target = area;
        }
        public void OnViewRangeExited(Area2D _)
        {
            if (IsDead) return;
            _target = null;
            Body.NextState = PigInBox.State.Idle;
            NextState = State.Idle;
        }
        #endregion
        public enum State
        {
            NoState = 0,
            Idle,
            Attack
        }
    }
}
