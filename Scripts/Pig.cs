﻿using System;
using Godot;
using kingsandpigs.Scripts.Common;

namespace kingsandpigs.Scripts
{
    public class Pig : BaseStatedBody<Pig.State>
    {
        protected new float AtkCD = 2f;
        public override void _Ready()
        {
            Speed = 160;
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
            AtkCD -= delta;
        }

        private State Idle()
        {
            if (AtkHandler()) return State.Attack;
            if (Mathf.Abs(Velocity.x) > 8) return State.Run;
            SpeedHandler();
            return CurState;
        }

        private State Attack()
        {
            SpeedHandler();
            return CurState;
        }

        private State Dead()
        {
            SpeedHandler();
            return CurState;
        }

        private State Fall()
        {
            SpeedHandler();
            return CurState;
        }

        private State Ground()
        {
            SpeedHandler();
            return CurState;
        }

        private State Hit()
        {
            SpeedHandler(.05f);
            if (IsOnFloor())
                return Health > 0 ? State.Hit : State.Dead;
            return CurState;
        }

        private State Run()
        {
            SpeedHandler();
            if (Mathf.Abs(Velocity.x) < 8) return State.Idle;
            return CurState;
        }

        private State Jump()
        {
            SpeedHandler();
            return CurState;
        }

        public bool AtkHandler()
        {
            if (CanAttack && AtkCD <= 0)
            {
                AtkCD = 2f;
                return true;
            }
            return false;
        }
        public void MovementHandler(float dir)
        {
            if (CurState == State.Hit || CurState == State.Dead || CurState == State.Attack) return;
            SpriteAnchor.Scale = new Vector2(-dir, 1);
            Velocity.x = Mathf.Lerp(Velocity.x, dir * Speed, 0.2f);
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
                State.Ground => State.Idle,
                State.Hit => Health > 0 ? State.Idle : State.Dead,
                _ => NextState
            };
        }
        #endregion
    }
}
