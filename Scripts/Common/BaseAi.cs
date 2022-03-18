using Godot;

namespace kingsandpigs.Scripts.Common
{
    public class BaseAi<TBody> : Node
    {
        private TBody _body;

        public override void _Ready()
        {
        }

        enum State
        {
            Idle = 0,
            Wander
        }
    }

}