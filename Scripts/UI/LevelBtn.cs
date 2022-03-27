using System;
using Godot;
using kingsandpigs.Scripts.Common;

public class LevelBtn : Button
{
    public int Level = 1;
    public event Action<int> OnBtnClick;
    private SfxMgr _sfx;
    public override void _Ready()
    {
        Text = Level.ToString();
        Disabled = Level > GlobalVar.UnlockedLevel;
        _sfx = GetChild<SfxMgr>(0);
    }

    public void OnClick()
    {
        _sfx.Play("Click");
        OnBtnClick?.Invoke(Level);
    }

    public void OnHover()
    {
        _sfx.Play("Hover");
    }
}
