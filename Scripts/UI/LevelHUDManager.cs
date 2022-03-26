using System;
using System.Collections.Generic;
using Godot;
using kingsandpigs.Scripts.Common;

namespace kingsandpigs.Scripts.UI
{
    public class LevelHUDManager : CanvasLayer
    {
        private TextureRect _liveBar;
        private Label _diamondLabel;
        private readonly List<SmallHeart> _hearts = new List<SmallHeart>();
        public int DiamondNum = 0;
        private AnimationPlayer _transitionPlayer;
        private AnimationPlayer _menu;
        public Action OnNextLevel;
        public override void _Ready()
        {
            InitHeart();
            _liveBar = GetChild<TextureRect>(0);
            _diamondLabel = GetChild<Label>(1);
            _menu = GetChild<AnimationPlayer>(2);
            _transitionPlayer = GetChild<AnimationPlayer>(3);
            _hearts.ForEach(h => _liveBar.AddChild(h));
            DiamondChange(GlobalVar.Diamond);
            TransOut();
        }

        public override void _PhysicsProcess(float delta)
        {
            if (Input.IsActionJustPressed("pause")) Pause();
        }

        private void InitHeart()
        {
            var smallHeart = GD.Load<PackedScene>("res://Nodes/HUD/SmallHeart.tscn");
            for (var i = 0; i < 3; i++)
            {
                var heart = smallHeart.Instance<SmallHeart>();
                heart.Position = new Vector2(i * 11 + 11, 10);
                _hearts.Add(heart);
            }
        }

        public void HealthChange(int newVal, int oldVal)
        {
            if (newVal < oldVal) _hearts[newVal].Hit();
            else if (newVal > oldVal) _hearts[oldVal].Recover();
        }

        public void DiamondChange(int num)
        {
            DiamondNum += num;
            _diamondLabel.Text = DiamondNum.ToString();
        }

        public void TransIn()
        {
            _transitionPlayer.Play("TriIn");
        }

        public void TransOut()
        {
            _transitionPlayer.Play("TriOut");
        }

        public void Pause()
        {
            var status = GetTree().Paused;
            if (status)
                _menu.Play("Out");
            else
            {
                _menu.Play("In");
                GetTree().Paused = !status;
            }
        }

        public void Menu()
        {
            OnNextLevel = () => GetTree().ChangeScene("res://Scene/MainMenu.tscn");
            _transitionPlayer.Play("TriIn");
        }

        public void OnTransitionStop(string name)
        {
            if (name.Contains("In"))
            {
                GD.Print("In");
                OnNextLevel?.Invoke();
            }
        }

        public void OnMenuOut(string name)
        {
            if (name is "Out")
                GetTree().Paused = false;
        }
    }
}
