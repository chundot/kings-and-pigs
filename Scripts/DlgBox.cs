using Godot;

namespace kingsandpigs.Scripts
{
    public class DlgBox : AnimatedSprite
    {
        private float _time;


        public override void _PhysicsProcess(float delta)
        {
            if (_time < 0 && Animation.Contains("In")) Play(Animation.Replace("In", "Out"));
            else _time -= delta;
        }

        public void Display(DlgType type, float time = 1f)
        {
            Visible = true;
            Play(type.ToString());
            _time = time;
        }

        public void AnimationFinished()
        {
            if (Animation.Contains("Out"))
                Visible = false;
        }
    }

    public enum DlgType
    {
        AttackIn = 0,
        AttackOut,
        BoomIn,
        BoomOut,
        DeadIn,
        DeadOut,
        ExcIn,
        ExcOut,
        HelloIn,
        HelloOut,
        HiIn,
        HiOut,
        ItgIn,
        ItgOut,
        LoserIn,
        LoserOut,
        NoIn,
        NoOut,
        WtfIn,
        WtfOut
    }
}

