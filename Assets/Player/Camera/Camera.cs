using Godot;

public partial class Camera : Camera2D
{
	[Export] private Node2D target = new();

	private Vector2 position = Vector2.Zero;

    public override void _Ready()
    {
        Position = target.Position;
    }

    public override void _Process(double delta)
	{
		Vector2 targetPos = target.Position;

		position.X = Position.Lerp(targetPos, (float)delta * 6.1f).X;
		position.Y = Position.Lerp(targetPos, (float)delta * 1.3f).Y;

		Position = position;
	}
}