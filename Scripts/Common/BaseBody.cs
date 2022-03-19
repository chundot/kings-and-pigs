using System;
using Godot;

namespace kingsandpigs.Scripts.Common
{
    public class BaseBody : KinematicBody2D
    {
        public int MaxHealth = 3;
        public int Health = 3;
        public bool CanAttack = false;
        protected Vector2 Velocity = Vector2.Zero;
        protected int Gravity = 12;
        protected int MaxFallSpeed = 360;
        protected int Speed = 120;
        protected int JumpForce = 280;
        protected Position2D SpriteAnchor;
        protected AnimationPlayer Player;
        protected DlgBox Dlg;
        protected float InvincibleTimer = 0.4f;
        protected float AtkCD = .4f;
        protected Node2D Info;
        public event Action<int, int> OnHealthChange;

        public override void _Ready()
        {
            Health = MaxHealth;
            SpriteAnchor = GetChild<Position2D>(0);
            Dlg = GetChild<DlgBox>(4);
            Player = GetChild<AnimationPlayer>(1);
            Info = GetChild<Node2D>(2);
            OnHealthChange += (newVal, oldVal) => Health = newVal;
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
            Velocity.y = Mathf.Clamp(Velocity.y + Gravity, -JumpForce, MaxFallSpeed);
        }

        protected void SpeedHandler(float factor = 0.2f)
        {
            Velocity.x = Mathf.Lerp(Velocity.x, 0, factor);
        }


        public virtual void HealthChange(int dmg)
        {
            var newVal = Mathf.Clamp(Health - dmg, 0, MaxHealth);
            OnHealthChange?.Invoke(newVal, Health);
        }

        #region SLOTS

        public virtual void OnAttackBoxEnter(Node2D body)
        {
            if (body is TileMap tileMap)
            {
                GD.Print(body.GetType().Name);
            }
        }
        public virtual void OnHitBoxEnter(Area2D area)
        {
            var isDmgFromAttack = area.GetLayerBit(LayerEnum.AttackBox);
            if (isDmgFromAttack && InvincibleTimer <= 0)
            {
                InvincibleTimer = 0.3f;
                HealthChange(1);
                Velocity = (Vector2.Right * (GlobalPosition.x - area.GlobalPosition.x)).Normalized() * Speed + Vector2.Up * JumpForce / 2;
                if (Health == 0)
                {
                    Dlg.Display(DlgType.DeadIn);
                    area.GetParent<Position2D>().GetParent<BaseBody>().Dlg.Display(DlgType.LoserIn);
                }
                else Dlg.Display(DlgType.WtfIn);
            }
        }

        #endregion

    }
}