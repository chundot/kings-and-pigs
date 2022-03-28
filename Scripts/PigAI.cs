using System;
using Godot;
using kingsandpigs.Scripts.Common;

namespace kingsandpigs.Scripts
{
    public class PigAI : BaseAI<Pig, PigAI.State>
    {
        private Area2D _target;
        private int _dir = 1;
        private Random _rnd = new Random();
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
                State.Follow => Follow(),
                State.TryAttack => TryAttack(),
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
            TransTimer -= delta;
            if (TransTimer < 0)
            {
                TransTimer = _rnd.Next(3, 5);
                return State.Wander;
            }
            return CurState;
        }
        private State Wander(float delta)
        {
            if (TransTimer < 0)
            {
                TransTimer = _rnd.Next(3, 6);
                return State.Idle;
            }
            TransTimer -= delta;
            if (Body.IsOnWall() || (Body.IsOnFloor() && !RayCast.IsColliding())) _dir = -_dir;
            Body.MovementHandler(_dir);
            return CurState;
        }
        private State Follow()
        {
            if (_target is null)
            {
                TransTimer = 1f;
                return State.Idle;
            }
            var xDelta = GlobalPosition.x - _target.GlobalPosition.x;
            _dir = xDelta > 0 ? -1 : 1;
            if (Mathf.Abs(xDelta) > 4f) Body.MovementHandler(_dir);
            if (Body.IsOnFloor() && _target.GlobalPosition.y < GlobalPosition.y)
                if (!RayCast.IsColliding()) Body.JumpHandler();
            return CurState;
        }
        private State TryAttack()
        {
            return CurState;
        }

        #region SLOTS
        public void OnAtkRangeEntered(Area2D _)
        {
            if (IsDead) return;
            Body.CanAttack = true;
            NextState = State.TryAttack;
        }

        public void OnAtkRangeExited(Area2D _)
        {
            if (IsDead) return;
            Body.CanAttack = false;
            NextState = State.Follow;
        }

        public void OnViewRangeEntered(Area2D area)
        {
            if (IsDead) return;
            Body.Dlg.Display(DlgType.ExcIn);
            _target = area;
            NextState = State.Follow;
        }
        public void OnViewRangeExited(Area2D _)
        {
            if (IsDead) return;
            _target = null;
            Body.Dlg.Display(DlgType.ItgIn);
            NextState = State.Idle;
        }
        #endregion
        public enum State
        {
            NoState = 0,
            Idle,
            Wander,
            Follow,
            TryAttack
        }
    }
}
