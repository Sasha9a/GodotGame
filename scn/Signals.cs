using Godot;
using System;

public partial class Signals : Node {
	[Signal]
	public delegate void PlayerPositionUpdateEventHandler(Vector2 position);

	[Signal]
	public delegate void EnemyAttackEventHandler(float enemyDamage);
}
