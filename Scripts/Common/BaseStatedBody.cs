using System;
using Godot;

namespace kingsandpigs.Scripts.Common
{
    public class BaseStatedBody<TEnum> : BaseBody where TEnum : Enum
    {
        public TEnum CurState;
        public TEnum NextState = default;

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
    }
}
