using System;
using Godot;

namespace kingsandpigs.Scripts.Common;

public class BaseSprite<TEnum> : KinematicBody2D where TEnum : Enum
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
    protected TEnum CurState;
    protected TEnum NextState = default;
    private Node2D Info;

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
        StateUpdate();
        GravityHandler();
        Velocity = MoveAndSlide(Velocity, Vector2.Up);
    }

    protected virtual void StateUpdate()
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

    protected virtual TEnum TransTo(TEnum nextState)
    {
        DebugInfo(nextState);
        BeforeTrans(nextState);
        CurState = nextState;
        PostTrans();
        return CurState;
    }

    protected virtual void BeforeTrans(TEnum nextState)
    {
    }

    protected virtual void PostTrans()
    {
        Player.Play(CurState.ToString());
    }

    private void DebugInfo(TEnum nextState)
    {
        Info.GetChild<Label>(0).Text = nextState.ToString();
    }

    public virtual void HealthChange(int dmg)
    {
        var newVal = Mathf.Clamp(health - dmg, 0, maxHealth);
        OnHealthChange?.Invoke(newVal, health);
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
        GD.Print(area.GetType().Name);
    }

    #endregion
}