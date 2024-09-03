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
	
	private DirectionalLight2D _sunLight;
	private PointLight2D _lightShop;
	private Label _dayLabel;
	private AnimationPlayer _lightAnimation;
	private AnimationPlayer _textAnimation;

	public override void _Ready() {
		_lightShop = GetNode<PointLight2D>("Light/LightShop");
		_dayLabel = GetNode<Label>("CanvasLayer/DayLabel");
		_textAnimation = GetNode<AnimationPlayer>("CanvasLayer/TextAnimation");
		_lightAnimation = GetNode<AnimationPlayer>("Light/LightAnimation");
		_sunLight = GetNode<DirectionalLight2D>("Light/Sun");
		_sunLight.Enabled = true;
		_dayCount = 1;
		SetDayText();
		DayTextShow();
	}

	private void MorningState() {
		_lightAnimation.Play("set_light");
	}

	private void EveningState() {
		_lightAnimation.Play("set_night");
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
		_textAnimation.Play("day_text_fade_in");
		GetTree().CreateTimer(3f).Timeout += DayTextHide;
	}

	private void DayTextHide() {
		_textAnimation.Play("day_text_fade_out");
	}
}
