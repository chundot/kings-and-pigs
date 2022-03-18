using Godot;

namespace kingsandpigs.Scripts
{
    public class DlgBox : AnimatedSprite
    {
        private Timer _timer;

        public override void _Ready()
        {
            _timer = GetChild<Timer>(0);
        }

        public void Display(DlgType type, float time = .8f)
        {
            Visible = true;
            Play(type.ToString());
            _timer.Start(time);
        }

        public void TimeOut()
        {
            if (Animation.Contains("In"))
            {
                Play(Animation.Replace("In", "Out"));
            }
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

