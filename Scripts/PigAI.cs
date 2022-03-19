using Godot;
using kingsandpigs.Scripts.Common;

namespace kingsandpigs.Scripts
{
    public class PigAI : Node2D
    {
        private BaseBody _pig;
        private BaseBody _target;
        private State CurState;
        public override void _Ready()
        {
            _pig = GetChild<BaseBody>(0);
        }

        public override void _PhysicsProcess(float delta)
        {
            CurState = CurState switch
            {
                State.Idle => Idle(),
                State.Wander => Wander(),
                State.Follow => Follow(),
                State.TryAttack => TryAttack(),
                _ => CurState
            };
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
            return CurState;
        }
        private State TryAttack()
        {
            return CurState;
        }

        public void OnAtkRangeEntered(Area2D area)
        {
            _pig.CanAttack = true;
            CurState = State.TryAttack;
        }

        public void OnAtkRangeExited(Area2D area)
        {
            _pig.CanAttack = false;
            CurState = State.Idle;
        }

        enum State
        {
            Idle = 0,
            Wander,
            Follow,
            TryAttack
        }
    }
}
