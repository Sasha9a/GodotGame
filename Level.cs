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
	
	private DirectionalLight2D _light;
	private PointLight2D _pointLight;

	public override void _Ready() {
		_pointLight = GetNode<PointLight2D>("PointLight2D");
		_light = GetNode<DirectionalLight2D>("DirectionalLight2D");
		_light.Enabled = true;
	}

	public override void _Process(double delta) {
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
		if (_lightState == LightState.Night) {
			_lightState = LightState.Morning;
		}
		else {
			_lightState += 1;
		}
	}
}
