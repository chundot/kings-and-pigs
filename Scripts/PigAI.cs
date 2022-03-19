using Godot;
using kingsandpigs.Scripts.Common;

namespace kingsandpigs.Scripts
{
    public class PigAI : Node2D
    {
        private Pig _pig;
        private Area2D _target;
        private State CurState = State.Idle;
        private State NextState = default;
        private bool _isDead = false;
        public override void _Ready()
        {
            _pig = GetParent<Position2D>().GetParent<Pig>();
            _pig.OnDeath = () =>
            {
                QueueFree();
                _isDead = true;
            };
        }

        public override void _PhysicsProcess(float delta)
        {
            var state = CurState switch
            {
                State.Idle => Idle(),
                State.Wander => Wander(),
                State.Follow => Follow(),
                State.TryAttack => TryAttack(),
                _ => CurState
            };
            if (NextState != default)
            {
                CurState = NextState;
                NextState = default;
            }
            else CurState = state;

        }

        private State Idle()
        {
            return CurState;
        }
        private State Wander()
        {
            return CurState;
        }
        private State Follow()
        {
            if (_target is null) return State.Idle;
            var dir = GlobalPosition.x > _target.GlobalPosition.x ? -1 : 1;
            _pig.MovementHandler(dir);
            return CurState;
        }
        private State TryAttack()
        {
            return CurState;
        }
        #region SLOTS
        public void OnAtkRangeEntered(Area2D _)
        {
            if (_isDead) return;
            _pig.CanAttack = true;
            CurState = State.TryAttack;
        }

        public void OnAtkRangeExited(Area2D _)
        {
            if (_isDead) return;
            _pig.CanAttack = false;
            CurState = State.Follow;
        }

        public void OnViewRangeEntered(Area2D area)
        {
            if (_isDead) return;
            _pig.Dlg.Display(DlgType.ExcIn);
            _target = area;
            CurState = State.Follow;
        }
        public void OnViewRangeExited(Area2D _)
        {
            if (_isDead) return;
            _pig.Dlg.Display(DlgType.ItgIn);
            CurState = State.Idle;
        }
        #endregion
        enum State
        {
            NoState = 0,
            Idle,
            Wander,
            Follow,
            TryAttack
        }
    }
}
