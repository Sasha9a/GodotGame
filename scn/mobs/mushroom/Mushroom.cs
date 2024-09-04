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

	private Vector2 _player;
	private Vector2 _direction;
	private float _damage = 20f;

	private Signals _signals;
	private AnimationPlayer _animationPlayer;
	private AnimatedSprite2D _animatedSprite;
	private CollisionShape2D _attackRangeCollision;
	private Node2D _attackDirection;
	
	public override void _Ready() {
		_signals = GetNode<Signals>("/root/Signals");
		_signals.Connect(Signals.SignalName.PlayerPositionUpdate, Callable.From((Vector2 position) => _on_player_position_update(position)));
		
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_attackRangeCollision = GetNode<CollisionShape2D>("AttackDirection/AttackRange/CollisionShape2D");
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_attackDirection = GetNode<Node2D>("AttackDirection");
	}

	public override void _PhysicsProcess(double delta) {
		if (!IsOnFloor()) {
			Velocity += GetGravity() * (float)delta;
		}

		if (State == StateEnum.Chase) {
			ChaseState();
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

	private void _on_player_position_update(Vector2 position) {
		_player = position;
	}

	private void ChaseState() {
		_direction = (_player - Position).Normalized();
		if (_direction.X < 0f) {
			_animatedSprite.FlipH = true;
			_attackDirection.RotationDegrees = 180f;
		}
		else if (_direction.X > 0f) {
			_animatedSprite.FlipH = false;
			_attackDirection.RotationDegrees = 0f;
		}
	}

	private void _on_hit_box_area_entered(Area2D area) {
		_signals.EmitSignal(Signals.SignalName.EnemyAttack, _damage);
	}
}
