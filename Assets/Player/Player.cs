using Godot;

public partial class Player : CharacterBody2D
{
	[Export] private float ACCELERATION = 150;
	[Export] private float DECELERATION = 0.78f;
	[Export] private float MAX_SPEED = 700;
	[Export] private float JUMP_STRENGTH = 1000;
	[Export] private float DASH_SPEED = 2200;
	[Export] private float DASH_DURATION = 0.12f;

 	private Vector2 velocity = Vector2.Zero;
	private Vector2 inputDir = Vector2.Zero;
	private Vector2 lastDir = Vector2.Right;

	private Vector2 gravityVector = new Vector2(0, 1);
	private float gravity = 2500;

	private float dashTimer;
	private bool dashing = false;

    public override void _Ready()
    {
        dashTimer = DASH_DURATION; // inizialize the first timer
    }
    public override void _PhysicsProcess(double delta)
	{
		// gets horizontal and vertical input
        float inputX = Input.GetAxis("left", "right");
        float inputY = -Input.GetAxis("down", "up");
        
        // uses a single vec2 var to store that input, not normalized by choice otherwise going diagonal would slow down the character
        inputDir = new Vector2(inputX, inputY);
		velocity = Velocity;

		if(dashing) // if in dashing state
		{
			velocity = lastDir.Normalized() * DASH_SPEED; // dashes in the last direction
			
			if(dashTimer <= 0) // if ifnisched dashing
			{
				dashing = false; // exit dashing state
				velocity.Y = 0; // reset vertical velocity
				dashTimer = DASH_DURATION; // reset timer for the next dash
			}
			else // else decrease timer
				dashTimer -= (float)delta;

		}
		else
		{
			velocity.X += inputDir.X * ACCELERATION; // accelerate
			velocity.X = Mathf.Clamp(velocity.X, -MAX_SPEED, MAX_SPEED); // stop at max speed

			if(inputDir != Vector2.Zero) // if moving change last dir
				lastDir = inputDir;
			
			if(inputDir.X == 0)// if not moving horizontally decelerate
				velocity.X *= DECELERATION;

			// if press dash on air enter dashing state
			if(Input.IsActionJustPressed("dash") && !IsOnFloor())
				dashing = true;

			// jump
			if(Input.IsActionJustPressed("jump") && IsOnFloor())
				velocity.Y = -JUMP_STRENGTH;

			// gravity
			if(!IsOnFloor())
				velocity += gravityVector * gravity * (float)delta;	
		}

		Velocity = velocity;
		// DebugHUD.Instance.Text = Position.ToString();
		MoveAndSlide();
	}
}