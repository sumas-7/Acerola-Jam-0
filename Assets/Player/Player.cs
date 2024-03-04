using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float ACCELERATION;
	[Export] public float MAX_SPEED;
	[Export] public float DAMPING;
	[Export] public float JUMP_STRENGTH;
	[Export] public float DASH_SPEED;
	[Export] public float DASH_DURATION;

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
				velocity = Vector2.Zero; // reset velocity
				dashTimer = DASH_DURATION; // reset timer for the next dash
			}
			else // else decrease timer
				dashTimer -= (float)delta;

		}
		else
		{
			if(inputDir != Vector2.Zero) // if moving change last dir
				lastDir = inputDir;
			
			if(inputDir.X == 0)// if not moving horizontally decelerate
				velocity.X *= DAMPING;

			if(Input.IsActionJustPressed("dash") && !IsOnFloor()) // if press dash on air enter dashing state
				dashing = true;

			velocity.X += inputDir.X * ACCELERATION; // accelerate
			velocity.X = Mathf.Clamp(velocity.X, -MAX_SPEED, MAX_SPEED); // stop at max speed

			// jump
			if(Input.IsActionJustPressed("jump") && IsOnFloor())
				velocity.Y = -JUMP_STRENGTH;

			// gravity
			if(!IsOnFloor())
				velocity += gravityVector * gravity * (float)delta;	
		}

		Velocity = velocity;
		DebugHUD.Instance.Text = velocity.ToString() + "\n" + inputDir;
		MoveAndSlide();
		
		// changes fps with tab press
		if(Input.IsActionPressed("test"))
            Engine.MaxFps = 20;
        else
            Engine.MaxFps = 60;
	}
}