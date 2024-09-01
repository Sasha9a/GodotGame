using Godot;
using System;

public partial class Gold : Area2D
{
	private void _on_body_entered(Node body) {
		if (body is not Player player) {
			return;
		}
		
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", Position - new Vector2(0, 25f), 0.3f);
		tween.TweenCallback(Callable.From(QueueFree));
		
		Tween tween1 = GetTree().CreateTween();
		tween1.TweenProperty(this, "modulate:a", 0, 0.3f);
		
		player.GiveGold(1);
	}
}
