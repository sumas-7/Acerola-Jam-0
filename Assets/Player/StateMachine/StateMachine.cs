using System.Collections.Generic;
using Godot;

public partial class StateMachine : Node
{
    // state variables
    [Export] public State initialState; // the initial state, set in the editor
	public State currentState; // the current state the machine is in
	public Dictionary<string, State> states = new Dictionary<string, State>(); // a dictionary of states as child of the machine
    
    // player variables
    [Export] public float ACCELERATION = 150;
	[Export] public float DECELERATION = 0.78f;
	[Export] public float MAX_SPEED = 750;
	[Export] public float JUMP_STRENGTH = 1000;
	[Export] public float JUMP_BUFFER_TIME = 0.18f;
	[Export] public float COYOTE_TIME = 0.1f;
	[Export] public float DASH_SPEED = 2300;
	[Export] public float DASH_DURATION = 0.13f;
    [Export] public float gravity = 3500;
    public float jumpBuffer = 0;
    public float coyoteTime = 0;
    public bool canJump = true;
    public bool canDash = true;
    public Vector2 velocity;
    public Vector2 lastDir = Vector2.Right;
    public CharacterBody2D player;
    
    // input variables
    public float inputX;
    public float inputY;
    public Vector2 inputDir;

    // animation variables
    public AnimationPlayer animPlayer;

    // audio variables
    public AudioStreamPlayer jumpSoundPlayer, dashSoundPlayer, landSoundPlayer;
    public AudioStream aberrantJump = (AudioStream)GD.Load("res://Assets/SFX/Dash0.wav");
    public AudioStream aberrantDash = (AudioStream)GD.Load("res://Assets/SFX/Jump0.wav");

    // various variables
    public string TRANSITION_STRING { get; } = "Transition";

    public override void _Ready()
    {
        player = GetOwner<CharacterBody2D>();

		for(int i = 0; i < GetChildCount(); i++) // for every child in the state machine
		{
			Node child = GetChild(i);

			if(child is State) // if the child is a State add it to the dictionary
			{
				states.Add(child.Name, (State)child);
				states[child.Name].Transition += OnTransition; // connects Transition signal to method OnTransition()
			}
		}

        // foreach(var state in states)
        //     GD.Print(state.Key);

        animPlayer = (AnimationPlayer)GetParent().GetChild(5);

        jumpSoundPlayer = (AudioStreamPlayer)GetParent().GetChild(2);
        dashSoundPlayer = (AudioStreamPlayer)GetParent().GetChild(3);
        landSoundPlayer = (AudioStreamPlayer)GetParent().GetChild(4);
        
        if(initialState != null) // if there is an initial state set in the editor, use it as the current state
        {
            initialState.StateEnter();
            currentState = initialState;
        }

    }
    public override void _Process(double delta)
    {
        // gets horizontal and vertical input
        inputX = Input.GetAxis("left", "right");
        inputY = -Input.GetAxis("down", "up");
        
        // uses a single vec2 var to store that input, not normalized by choice otherwise going diagonal would slow down the character
        inputDir = new Vector2(inputX, inputY);

        // decrease various timers
        jumpBuffer -= (float)delta;
        coyoteTime -= (float)delta;

        if(currentState != null) // if there's a current state, call its process function
			currentState.StateProcess(delta);
    }
    public override void _PhysicsProcess(double delta)
    {
        velocity = player.Velocity;

        // jump logic
		if(Input.IsActionJustPressed("jump"))
			jumpBuffer = JUMP_BUFFER_TIME;

        if(canJump && jumpBuffer > 0 && (coyoteTime >= 0 || player.IsOnFloor()))
        {
			velocity.Y = -JUMP_STRENGTH; // changes Y velocity making the player jump
            animPlayer.Stop();
            animPlayer.Play("Jump");
            jumpSoundPlayer.Play();
            jumpBuffer = 0; // resets the jump buffer to avoid jumping as soon as you dash on the ground
            canJump = false; // remove the ability of jumping again
        }

        if(currentState != null) // if there's a current state, call its physics process function
			currentState.StatePhysicsProcess(delta);

        player.Velocity = velocity;
        player.MoveAndSlide();
    }

	public void OnTransition(State callingState, string newStateName) // function called when the state emits the transition signal
    {
        if (callingState == currentState) // only the current state should be able to transition to a new one
        {
            State newState = new();
            states.TryGetValue(newStateName.ToLower(), out newState);
            if (newState != null) // if the new state exist
            {
                if (currentState != null) // if there's already a current state, call its Exit function
                    currentState.StateExit();

                newState.StateEnter(); // call the new state enter function

                currentState = newState; // set the new state as the current one
            }
        }
    }

    // function to call when the state includes moving
	public void ApplyVerticalMovement(double delta)
    {
		velocity.X += inputDir.X * ACCELERATION; // accelerate
        velocity.X = Mathf.Clamp(velocity.X, -MAX_SPEED, MAX_SPEED); // stop at max speed
        
        if(inputDir.X == 0)// if not moving horizontally decelerate
            velocity.X *= DECELERATION;

        if(inputDir != Vector2.Zero) // if moving change last dir
            lastDir = inputDir;
    }
	// function to call when the state includes gravity
    public void ApplyGravity(double delta)
    {
        if(!player.IsOnFloor())
			velocity.Y += gravity * (float)delta;
    }
}