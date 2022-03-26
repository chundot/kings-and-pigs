using System;
using Godot;

namespace kingsandpigs.Scripts.Common
{
    public class BaseBody : KinematicBody2D
    {
        [Export] public int MaxHealth = 3;
        public int Health = 3;
        public bool CanAttack = false;
        protected Vector2 Velocity = Vector2.Zero;
        protected int Gravity = 12;
        protected int MaxFallSpeed = 360;
        [Export] protected int Speed = 120;
        [Export] protected int JumpForce = 280;
        protected Position2D SpriteAnchor;
        protected AnimationPlayer Player;
        public DlgBox Dlg;
        protected float InvincibleTimer = .4f;
        protected float AtkCD = 1f;
        protected float FallTimer = -.1f;
        public float GravityTimer = -.1f;
        protected Node2D Info;
        public event Action<int, int> OnHealthChange;
        public Action<int> OnDiamondChanged;
        public Action OnDeath;
        protected BaseBody KilledBy;

        public override void _Ready()
        {
            Health = MaxHealth;
            SpriteAnchor = GetChild<Position2D>(0);
            Dlg = GetChild<DlgBox>(4);
            Player = GetChild<AnimationPlayer>(1);
            Info = GetChild<Node2D>(2);
            OnHealthChange += (newVal, oldVal) => Health = newVal;
            OnDeath += () =>
            {
                if (KilledBy != null) KilledBy.Dlg.Display(DlgType.LoserIn);
                this.SetLayerBit(LayerEnum.Rb);
                this.SetLayerBit(LayerEnum.DeadBody);
            };
        }

        public override void _PhysicsProcess(float delta)
        {
            StateUpdate(delta);
            GravityHandler(delta);
            Velocity = MoveAndSlide(Velocity, Vector2.Up);
            InvincibleTimer = InvincibleTimer < -.1f ? InvincibleTimer : InvincibleTimer - delta;
            if (Velocity.y > 0)
                FallTimer = FallTimer > 0 ? FallTimer - delta : FallTimer;
        }

        protected virtual void StateUpdate(float delta)
        {
        }

        protected void GravityHandler(float delta)
        {
            var val = Velocity.y;
            if (Velocity.y > 0) GravityTimer = -.1f;
            if (GravityTimer > 0)
            {
                val += Gravity / 2;
                GravityTimer -= delta;
            }
            else val += Gravity;
            Velocity.y = Mathf.Clamp(val, -JumpForce, MaxFallSpeed);
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
            }
        }
        public virtual void OnHitBoxEnter(Area2D area)
        {
            var isDmgFromAttack = area.GetLayerBit(LayerEnum.AttackBox);
            if (isDmgFromAttack && InvincibleTimer <= 0)
            {
                InvincibleTimer = 0.3f;
                HealthChange(1);
                Velocity = (Vector2.Right * (GlobalPosition.x - area.GlobalPosition.x)).Normalized() * Speed * 1.5f + Vector2.Up * JumpForce / 2.5f;
                if (Health == 0)
                    KilledBy = area.GetParent<Position2D>().GetParent<BaseBody>();
                else Dlg.Display(DlgType.WtfIn);
            }
        }

        public virtual void OnBodyEntered(Node2D body)
        {
            if (body is Diamond d)
            {
                if (d.CurState is Diamond.State.Hit) return;
                d.ToHit();
                OnDiamondChanged(1);
            }
            else if (body is Heart h)
            {
                if (h.Hit || Health == MaxHealth) return;
                h.ToHit();
                OnHealthChange(Health + 1, Health);
            }
        }

        #endregion

    }
}