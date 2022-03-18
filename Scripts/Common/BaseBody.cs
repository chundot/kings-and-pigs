using System;
using Godot;

namespace kingsandpigs.Scripts.Common
{
    public class BaseBody : KinematicBody2D
    {
        public int maxHealth = 3;
        public int health = 3;
        protected Vector2 Velocity = Vector2.Zero;
        protected int Gravity = 12;
        protected int Speed = 120;
        protected int JumpForce = 280;
        protected Position2D SpriteAnchor;
        protected AnimationPlayer Player;
        protected DlgBox Dlg;
        protected float InvincibleTimer = 0.4f;
        protected Node2D Info;
        public event Action<int, int> OnHealthChange;

        public override void _Ready()
        {
            health = maxHealth;
            SpriteAnchor = GetChild<Position2D>(0);
            Dlg = GetChild<DlgBox>(4);
            Player = GetChild<AnimationPlayer>(1);
            Info = GetChild<Node2D>(2);
            OnHealthChange += (newVal, oldVal) => health = newVal;
        }

        public override void _PhysicsProcess(float delta)
        {
            StateUpdate(delta);
            GravityHandler();
            Velocity = MoveAndSlide(Velocity, Vector2.Up);
            InvincibleTimer = InvincibleTimer < -.1f ? InvincibleTimer : InvincibleTimer - delta;
        }

        protected virtual void StateUpdate(float delta)
        {
        }

        protected void GravityHandler()
        {
            Velocity.y += Gravity;
        }

        protected void SpeedHandler(float factor = 0.2f)
        {
            Velocity.x = Mathf.Lerp(Velocity.x, 0, factor);
        }


        public virtual void HealthChange(int dmg)
        {
            var newVal = Mathf.Clamp(health - dmg, 0, maxHealth);
            OnHealthChange?.Invoke(newVal, health);
        }

    }
}