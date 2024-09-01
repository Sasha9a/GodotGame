using Godot;
using System;
using Range = Godot.Range;

public partial class Collectibles : Node2D {
	private PackedScene _gold;

	public override void _Ready() {
		_gold = ResourceLoader.Load<PackedScene>("res://Collectibles/gold.tscn");
	}
	
	private void _on_timer_timeout() {
		// Gold goldTemp = _gold.Instantiate<Gold>();
		//
		// if (goldTemp is null) {
		// 	return;
		// }
		//
		// goldTemp.Position = new Vector2(new RandomNumberGenerator().RandiRange(50, 500), 560);
		// AddChild(goldTemp);
	}
}
