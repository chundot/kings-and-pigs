using Godot;
using kingsandpigs.Scripts.Common;

namespace kingsandpigs.Scripts
{
    public class PigAI : BaseAI<Pig, PigAI.State>
    {
        private Area2D _target;
        public override void _Ready()
        {
            CurState = State.Idle;
            base._Ready();
        }

        protected override void StateUpdate()
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
            Body.MovementHandler(dir);
            if (Body.IsOnFloor())
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
