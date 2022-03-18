using Godot;
namespace kingsandpigs.Scripts.UI
{
    public class SmallHeart : AnimatedSprite
    {
        public void Hit()
        {
            Play("Hit");
        }

        public void Recover()
        {
            Visible = true;
            Play("Idle");
        }

        public void OnAnimationFinished()
        {
            if (Animation is "Hit") Visible = false;
        }

    }
}