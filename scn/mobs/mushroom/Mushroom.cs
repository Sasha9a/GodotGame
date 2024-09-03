using Godot;
using System;

public partial class Mushroom : CharacterBody2D {
	public enum StateEnum {
		Idle,
		Attack,
		Chase
	}

	private StateEnum _state = StateEnum.Idle;
	private StateEnum State {
		get => _state;
		set
		{
			_state = value;
			switch (value) {
				case StateEnum.Idle: {
					IdleState();
					break;
				}
				case StateEnum.Attack: {
					AttackState();
					break;
				}
			}
		}
	}

	private AnimationPlayer _animationPlayer;
	private CollisionShape2D _attackRangeCollision;
	
	public override void _Ready() {
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_attackRangeCollision = GetNode<CollisionShape2D>("AttackDirection/AttackRange/CollisionShape2D");
	}

	public override void _PhysicsProcess(double delta) {
		if (!IsOnFloor()) {
			Velocity += GetGravity() * (float)delta;
		}
		
		MoveAndSlide();
	}

	private void _on_attack_range_body_entered(Node2D body) {
		State = StateEnum.Attack;
	}

	private void IdleState() {
		_animationPlayer.Play("Idle");
		GetTree().CreateTimer(1f).Timeout += IdleFinished;
	}
	
	private void AttackState() {
		_animationPlayer.Play("Attack");
		_animationPlayer.AnimationFinished += AttackFinished;
	}

	private void AttackFinished(StringName name) {
		_animationPlayer.AnimationFinished -= AttackFinished;
		_attackRangeCollision.Disabled = true;
		State = StateEnum.Idle;
	}

	private void IdleFinished() {
		_attackRangeCollision.Disabled = false;
		State = StateEnum.Chase;
	}
}
