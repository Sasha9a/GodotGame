using Godot;
using System;

public partial class Level : Node2D
{
	private enum LightState {
		Morning,
		Day,
		Evening,
		Night
	}
	
	private LightState _lightState = LightState.Morning;
	private int _dayCount;
	
	private DirectionalLight2D _light;
	private PointLight2D _pointLight;
	private Label _dayLabel;
	private AnimationPlayer _animationPlayer;

	public override void _Ready() {
		_pointLight = GetNode<PointLight2D>("PointLight2D");
		_dayLabel = GetNode<Label>("CanvasLayer/DayLabel");
		_animationPlayer = GetNode<AnimationPlayer>("CanvasLayer/AnimationPlayer");
		_light = GetNode<DirectionalLight2D>("DirectionalLight2D");
		_light.Enabled = true;
		_dayCount = 1;
		SetDayText();
		DayTextShow();
	}

	private void MorningState() {
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(_light, "energy", 0.2f, 20f);
		
		Tween tween1 = GetTree().CreateTween();
		tween1.TweenProperty(_pointLight, "energy", 0f, 20f);
	}

	private void EveningState() {
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(_light, "energy", 0.95f, 20f);
		
		Tween tween1 = GetTree().CreateTween();
		tween1.TweenProperty(_pointLight, "energy", 1.5f, 20f);
	}

	private void _on_day_night_timeout() {
		switch (_lightState) {
			case LightState.Morning: {
				MorningState();
				break;
			}
			case LightState.Evening: {
				EveningState();
				break;
			}
		}
		
		if (_lightState == LightState.Night) {
			_lightState = LightState.Morning;
			_dayCount++;
			SetDayText();
			DayTextShow();
		}
		else {
			_lightState += 1;
		}
	}

	private void SetDayText() {
		_dayLabel.Text = "Day " + _dayCount;
	}

	private void DayTextShow() {
		_animationPlayer.Play("day_text_fade_in");
		GetTree().CreateTimer(3f).Timeout += DayTextHide;
	}

	private void DayTextHide() {
		_animationPlayer.Play("day_text_fade_out");
	}
}
