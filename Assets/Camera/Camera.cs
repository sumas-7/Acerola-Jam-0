using Godot;

public partial class Camera : Camera2D
{
	[Export] private Node2D target = new();

	private Vector2 position = Vector2.Zero;

    public override void _Ready()
    {
        target.Position = Position;
    }

    public override void _Process(double delta)
	{
		position.X = Position.Lerp(target.Position, (float)delta * 6.1f).X;
		position.Y = Position.Lerp(target.Position, (float)delta * 1.3f).Y;

		Position = position;
	}
}