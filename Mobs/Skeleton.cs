using Godot;
using System;

public partial class Skeleton : CharacterBody2D {
	private const float Speed = 100.0f;

	private bool _chase;
	private bool _alive = true;
	private AnimatedSprite2D _animNode;
	
	public override void _Ready() {
		_animNode = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor()) {
			velocity += GetGravity() * (float)delta;
		}
		
		if (!_alive) {
			return;
		}
		
		Node scene = GetTree().CurrentScene;
		Player player = scene.GetNode<Player>("Player/Player");
		Vector2 direction = (player.Position - Position).Normalized();

		if (_chase) {
			velocity = new Vector2(direction.X * Speed, velocity.Y);
			_animNode.Play("Run");
		}
		else {
			velocity = new Vector2(0f, velocity.Y);
			_animNode.Play("Idle");
		}

		_animNode.FlipH = direction.X < 0f;

		Velocity = velocity;
		MoveAndSlide();
	}

	private void _on_detector_body_entered(Node body) {
		if (body is Player) {
			_chase = true;
		}
	}

	private void _on_detector_body_exited(Node body) {
		if (body is Player) {
			_chase = false;
		}
	}

	private void _on_death_body_entered(Node body) {
		if (body is not Player player) {
			return;
		}
		
		if (!_alive) {
			return;
		}

		player.Velocity = new Vector2(player.Velocity.X, player.Velocity.Y - 300f);
		_alive = false;
		_animNode.Play("Death");
		_animNode.AnimationFinished += Death;
	}

	private void _on_death_2_body_entered(Node body) {
		if (body is not Player player) {
			return;
		}

		if (!_alive) {
			return;
		}
		
		player.SetDamage(40f);
		_alive = false;
		_animNode.Play("Death");
		_animNode.AnimationFinished += Death;
	}

	private void Death() {
		_animNode.AnimationFinished -= Death;
		QueueFree();
	}
}
