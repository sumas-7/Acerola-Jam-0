using Godot;

public partial class Camera : Camera2D
{
	Vector2 position = Vector2.Zero;
	private CharacterBody2D player;

	public override void _Ready()
	{
		player = (CharacterBody2D)GetParent().GetNode("Player");
		GD.Print(player);
	}

	public override void _Process(double delta)
	{
		position.X = Position.Lerp(player.Position, 0.1f).X;
		position.Y = Position.Lerp(player.Position, 0.02f).Y;

		Position = position;
	}
}