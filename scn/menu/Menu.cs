using Godot;
using System;

public partial class Menu : Node2D {
	private void _on_quit_pressed() {
		GetTree().Quit();
	}

	private void _on_play_pressed() {
		GetTree().ChangeSceneToFile("res://scn/levels/level.tscn");
	}
}
