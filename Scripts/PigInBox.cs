using System;
using Godot;
using kingsandpigs.Scripts.Common;

namespace kingsandpigs.Scripts
{
    public class PigInBox : BaseStatedBody<PigInBox.State>
    {
        private int _dir = 1;
        private int _jmpDir = 1;
        public override void _Ready()
        {
            base._Ready();
            TransTo(State.Idle);
        }

        protected override void StateUpdate(float delta)
        {
            var state = CurState switch
            {
                State.Idle => Idle(),
                State.BeforeJump => BeforeJump(),
                State.Hit => Hit(),
                State.Fall => Fall(),
                State.Ground => Ground(),
                State.Jump => Jump(),
                State.LookingOut => LookingOut(),
                _ => CurState,
            };
            if (NextState != default)
            {
                state = NextState;
                NextState = default;
            }
            if (state != CurState) TransTo(state);
        }

        private State Idle()
        {
            SpeedHandler();
            return CurState;
        }

        private State BeforeJump()
        {
            SpeedHandler();
            if (Velocity.y < 0) return State.Jump;
            return CurState;
        }
        private State Hit()
        {
            SpeedHandler(.1f);
            return CurState;
        }
        private State Fall()
        {
            if (IsOnFloor()) return State.Ground;
            return CurState;
        }

        private State Ground()
        {
            SpeedHandler(.15f);
            return CurState;
        }

        private State Jump()
        {
            if (IsOnFloor()) return State.Ground;
            else if (Velocity.y > 0) return State.Fall;
            return CurState;
        }

        private State LookingOut()
        {
            SpeedHandler();
            return CurState;
        }

        public void ReadyJump(int dir)
        {
            if (CurState != State.Idle && CurState != State.LookingOut) return;
            _jmpDir = dir;
            NextState = State.BeforeJump;
        }

        private void GenerateFragments()
        {
            var rnd = new Random();
            var frag = GD.Load<PackedScene>("res://Nodes/Crate/CrateFrag.tscn");
            var parent = GetParent();
            for (var i = 0; i < 4; i++)
            {
                var node = frag.Instance<CrateFrag>();
                node.GlobalPosition = GlobalPosition;
                node.Set(i, Vector2.Right * _dir * rnd.Next(150, 240) + Vector2.Up * rnd.Next(80, 150));
                parent.AddChild(node);
            }
        }

        private void GeneratePig()
        {
            var pig = GD.Load<PackedScene>("res://Nodes/Prefabs/PigE.tscn");
            var parent = GetParent();
            var pigI = pig.Instance<Pig>();
            pigI.GlobalPosition = GlobalPosition;
            parent.AddChild(pigI);
        }

        public void OnAnimationFinished(string name)
        {
            _ = Enum.TryParse(name, out State state);
            NextState = state switch
            {
                State.Ground => State.Idle,
                State.BeforeJump => State.Jump,
                _ => CurState
            };
            if (state is State.Hit)
            {
                GenerateFragments();
                GeneratePig();
                QueueFree();
            }
            else if (state is State.BeforeJump)
            {
                SpriteAnchor.Scale = new Vector2(-_jmpDir, 1);
                Velocity.x = _jmpDir * Speed * 2;
                Velocity.y = -JumpForce;
            }
        }

        public override void OnHitBoxEnter(Area2D area)
        {
            var isDmgFromAttack = area.GetLayerBit(LayerEnum.AttackBox);
            if (isDmgFromAttack && area.Name != "BoxAtkBox")
            {
                _dir = area.GlobalPosition.x > GlobalPosition.x ? -1 : 1;
                TransTo(State.Hit);
            }
        }

        public enum State
        {
            NoState = 0,
            Idle,
            BeforeJump,
            Hit,
            Fall,
            Ground,
            Jump,
            LookingOut
        }
    }
}