using Godot;
using System;

public partial class Mushroom : CharacterBody2D {
	private AnimationPlayer _animationPlayer;
	
	public override void _Ready() {
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public override void _PhysicsProcess(double delta) {
		if (!IsOnFloor()) {
			Velocity += GetGravity() * (float)delta;
		}
		
		MoveAndSlide();
	}

	private void _on_attack_range_body_entered(Node2D body) {
		_animationPlayer.Play("Attack");
	}
}
