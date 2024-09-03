using Godot;
using System;

public partial class Bg : ParallaxBackground {
	private const float Speed = 100.0f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		ScrollOffset = new Vector2(ScrollOffset.X - Speed * (float)delta, 0f);
	}
}
