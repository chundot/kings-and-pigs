using System;
using Godot;
using kingsandpigs.Scripts.Common;

namespace kingsandpigs.Scripts
{
    public class PigAI : BaseAI<Pig, PigAI.State>
    {
        private Area2D _target;
        private int _dir = 1;
        private readonly float _wanderFactor = .8f;
        private readonly Random _rnd = new();

        private float _followTimer = 4f;
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
                State.Follow => Follow(delta),
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
            if (TransTimer < 0)
            {
                if (Triggered)
                {
                    TransTimer = _rnd.Next(2, 4);
                    return State.Wander;
                }
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
            Body.PickHandler();
            if (Body.IsOnWall() || (Body.IsOnFloor() && !RayCast.IsColliding())) _dir = -_dir;
            Body.MovementHandler(_dir, _wanderFactor);
            return CurState;
        }
        private State Follow(float delta)
        {
            if (_target is null || _followTimer < 0)
            {
                TransTimer = 8f;
                return State.Idle;
            }
            _followTimer -= delta;
            var xDelta = GlobalPosition.x - _target.GlobalPosition.x;
            _dir = xDelta > 0 ? -1 : 1;
            if (Mathf.Abs(xDelta) > 4f) Body.MovementHandler(_dir);
            if (Body.IsOnFloor() && _target.GlobalPosition.y - 32 < GlobalPosition.y)
                if (!RayCast.IsColliding()) Body.JumpHandler();
            if (Body.IsOnWall()) Body.JumpHandler(1.2f);
            return CurState;
        }
        private State TryAttack()
        {
            if (_target != null)
            {
                _followTimer = 4f;
                var xDelta = GlobalPosition.x - _target.GlobalPosition.x;
                _dir = xDelta > 0 ? -1 : 1;
                Body.MovementHandler(_dir, 0);
            }
            return CurState;
        }

        #region SLOTS
        public void OnAtkRangeEntered(Area2D area)
        {
            if (IsDead) return;
            if (area.GetParent<Node2D>() is Bomb)
            {
                if (CurState is State.Follow)
                {
                    Body.CanAttack = true;
                    NextState = State.TryAttack;
                }
            }
            else if (area.GetLayerBit(LayerEnum.PlayerHitBox))
            {
                Body.CanAttack = true;
                NextState = State.TryAttack;
            }
        }

        public void OnAtkRangeExited(Area2D _)
        {
            if (IsDead) return;
            Body.CanAttack = false;
            NextState = State.Follow;
        }

        public void OnViewRangeEntered(Area2D area)
        {
            Triggered = true;
            if (IsDead) return;
            Body.Dlg.Display(DlgType.ExcIn);
            _target = area;
            CurState = State.Follow;
        }
        public void OnViewRangeExited(Area2D _)
        {
            _followTimer = 4f;
            if (IsDead) return;
            _target = null;
            Body.Dlg.Display(DlgType.ItgIn);
            CurState = State.Idle;
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
