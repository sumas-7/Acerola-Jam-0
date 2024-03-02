using Godot;

public partial class Player : CharacterBody2D
{
	// [Export] public const float SPEED = 300.0f;
	// [Export] public const float JUMP_STRENGTH = -400.0f;

	// public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	// public override void _PhysicsProcess(double delta)
	// {
	// 	if(Input.IsActionPressed("test"))
    //         Engine.MaxFps = 20;
    //     else
    //         Engine.MaxFps = 60;
		
	// 	Vector2 velocity = Velocity;

	// 	if(!IsOnFloor())
	// 		velocity.Y += gravity * (float)delta;

	// 	// jump
	// 	if(Input.IsActionJustPressed("jump") && IsOnFloor())
	// 		velocity.Y = JUMP_STRENGTH;

	// 	Vector2 direction = Input.GetVector("left", "right", "up", "down");
		
	// 	velocity.X = direction.X * SPEED;

	// 	DebugUI.Instance.Text = velocity.ToString();

	// 	Velocity = velocity;
	// 	MoveAndSlide();
	// }
}