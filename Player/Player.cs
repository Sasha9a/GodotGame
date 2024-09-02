using Godot;
using System;

public partial class Player : CharacterBody2D {
	private enum State {
		Move,
		Attack,
		Attack2,
		Attack3,
		Block,
		Slide
	}
	
	private const float Speed = 150.0f;
	private const float JumpVelocity = -400.0f;

	private AnimatedSprite2D _animNode;
	private AnimationPlayer _animPlayerNode;
	private float _health = 100f;
	private int _gold;
	private State _state = State.Move;
	private float _runSpeed = 0.5f;

	private bool _isAnimSetCallback;

	public override void _Ready() {
		_animNode = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animPlayerNode = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public override void _PhysicsProcess(double delta) {
		switch (_state) {
			case State.Move:
				MoveState();
				break;
			case State.Block:
				BlockState();
				break;
			case State.Slide:
				SlideState();
				break;
		}

		// Add the gravity.
		if (!IsOnFloor()) {
			Velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("attack") && IsOnFloor()) {
			Velocity = new Vector2(Velocity.X, JumpVelocity);
			_animPlayerNode.Play("Jump");
		}

		if (Velocity.Y > 0f) {
			_animPlayerNode.Play("Fall");
		}

		if (GetHealth() == 0f) {
			_animPlayerNode.Play("Death");
			_animPlayerNode.AnimationFinished += Death;
			return;
		}
		
		MoveAndSlide();
	}

	public float GetHealth() {
		return _health;
	}

	public void SetDamage(float damage) {
		_health = _health - damage <= 0f ? 0f : _health - damage;
	}
	
	public int GetGold() {
		return _gold;
	}

	public void GiveGold(int gold) {
		_gold += gold;
	}

	private void MoveState() {
		float direction = Input.GetAxis("left", "right");
		
		if (direction != 0f) {
			Velocity = new Vector2(direction * Speed * _runSpeed, Velocity.Y);
			if (Velocity.Y == 0f) {
				_animPlayerNode.Play(_runSpeed > 0.6f ? "Run" : "Walk");
			}
		}
		else {
			Velocity = new Vector2(Mathf.MoveToward(Velocity.X, 0, Speed), Velocity.Y);
			if (Velocity.Y == 0f) {
				_animPlayerNode.Play("Idle");
			}
		}

		if (direction <= -1f) {
			_animNode.FlipH = true;
		}
		else if (direction >= 1f) {
			_animNode.FlipH = false;
		}

		_runSpeed = Input.IsActionPressed("run") ? 1.5f : 0.5f;

		if (Input.IsActionPressed("block") && Velocity.X == 0f) {
			_state = State.Block;
		} else if (Input.IsActionPressed("block") && Velocity.X != 0f) {
			_state = State.Slide;
		}
	}

	private void BlockState() {
		Velocity = new Vector2(0, Velocity.Y);
		_animPlayerNode.Play("Block");
		if (Input.IsActionJustReleased("block")) {
			_state = State.Move;
		}
	}

	private void SlideState() {
		_animPlayerNode.Play("Slide");
		if (!_isAnimSetCallback) {
			_animPlayerNode.AnimationFinished += SlideFinish;
			_isAnimSetCallback = true;
		}
	}

	private void Death(StringName name) {
		_animPlayerNode.AnimationFinished -= Death;
		QueueFree();
		GetTree().ChangeSceneToFile("res://menu.tscn");
	}

	private void SlideFinish(StringName name) {
		_animPlayerNode.AnimationFinished -= SlideFinish;
		_isAnimSetCallback = false;
		_state = State.Move;
	}
}
