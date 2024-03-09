using Godot;

public partial class MovingSaw : Prop
{
	[Export] private float SPEED = 0.5f;
	[Export] private float ROTATION_SPEED = 12;
	[Export] private float PATH_FOLLOW_START;


	private Vector2 lastPos;
	private Vector2 direction;
	private Sprite2D sprite;
	private Path2D path; // takes path from parent
	private PathFollow2D pathFollow = new(); // create new pathFollow

	public override void _Ready()
	{
		base._Ready();

		path = GetParentOrNull<Path2D>();
		sprite = GetChild(0).GetChildOrNull<Sprite2D>(0);

		if(path != null)
			path.CallDeferred("add_child", pathFollow); // add path follow as child of path

		pathFollow.Ready += () => {pathFollow.ProgressRatio = PATH_FOLLOW_START;}; // changes the progressRatio in the pathFollow's ready
	}

	public override void _Process(double delta)
	{
		if(path != null) // if there's a path move with it
		{
			pathFollow.ProgressRatio += SPEED * (float)delta;
			GlobalPosition = pathFollow.GlobalPosition;
		}

		direction = (GlobalPosition - lastPos).Normalized();

		if(direction != Vector2.Zero) // if moving, change direction based on the dir 
			if(Mathf.Abs(direction.X) > Mathf.Abs(direction.Y)) // if is moving more horizontally rotate based on the x velocity
				sprite.Rotate(ROTATION_SPEED * direction.X * (float)delta);
			else // else do otherwise
				sprite.Rotate(ROTATION_SPEED * direction.Y * (float)delta);
		else // else just rotate
			sprite.Rotate(-ROTATION_SPEED * (float)delta);

		lastPos = GlobalPosition;
	}
}