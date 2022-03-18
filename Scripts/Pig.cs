using System;
using kingsandpigs.Scripts.Common;

namespace kingsandpigs.Scripts
{
    public class Pig : BaseStatedBody<Pig.State>
    {
        private float _timer = 2f;
        public override void _Ready()
        {
            base._Ready();
            OnHealthChange += (newVal, oldVal) => NextState = newVal < oldVal ? State.Hit : NextState;
            TransTo(State.Idle);
        }

        protected override void StateUpdate(float delta)
        {
            var state = CurState switch
            {
                State.Idle => Idle(),
                State.Attack => Attack(),
                State.Dead => Dead(),
                State.Fall => Fall(),
                State.Ground => Ground(),
                State.Hit => Hit(),
                State.Run => Run(),
                State.Jump => Jump(),
                _ => CurState
            };

            if (NextState != default)
            {
                state = NextState;
                NextState = default;
            }
            if (state != CurState) TransTo(state);
            _timer -= delta;
        }

        private State Idle()
        {
            if (_timer <= 0)
            {
                _timer = 2f;
                return State.Attack;
            }
            return CurState;
        }

        private State Attack()
        {
            return CurState;
        }

        private State Dead()
        {
            return CurState;
        }

        private State Fall()
        {
            return CurState;
        }

        private State Ground()
        {
            return CurState;
        }

        private State Hit()
        {
            return CurState;
        }

        private State Run()
        {
            return CurState;
        }

        private State Jump()
        {
            return CurState;
        }

        public enum State
        {
            NoState = 0,
            Idle,
            Attack,
            Dead,
            Fall,
            Ground,
            Hit,
            Run,
            Jump
        }

        #region SLOTS
        public void AnimationFinished(string name)
        {
            _ = Enum.TryParse(name, out State state);
            NextState = state switch
            {
                State.Attack => !IsOnFloor() ? State.Fall : State.Idle,
                State.Ground or State.Hit => State.Idle,
                _ => NextState
            };
        }
        #endregion
    }
}
