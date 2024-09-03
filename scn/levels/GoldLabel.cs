using Godot;
using System;

public partial class GoldLabel : Label
{
	public override void _Process(double delta) {
		Node scene = GetTree().CurrentScene;
		Player player = scene.GetNode<Player>("Player/Player");
		Text = "GOLD: " + player.GetGold();
	}
}
