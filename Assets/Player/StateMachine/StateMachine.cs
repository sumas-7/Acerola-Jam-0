using System.Collections.Generic;
using Godot;

public partial class StateMachine : Node
{
    // state variables
    [Export] public State initialState; // the initial state, set in the editor
	public State currentState; // the current state the machine is in
	public Dictionary<string, State> states = new Dictionary<string, State>(); // a dictionary of states as child of the machine
    
    // player variables
    [Export] public float speed = 0;
    [Export] public float JumpStrength = 0;
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    public Vector2 velocity;
    public CharacterBody2D player;
    
    // input variables
    public float inputX;
    public float inputY;
    public Vector2 inputDir;

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

        foreach(var state in states)
            GD.Print(state.Key);

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

        if(currentState != null) // if there's a current state, call its process function
			currentState.StateProcess(delta);
    }
    public override void _PhysicsProcess(double delta)
    {
        velocity = player.Velocity;

        if(currentState != null) // if there's a current state, call its physics process function
			currentState.StatePhysicsProcess(delta);

        DebugHUD.Instance.Text = velocity.ToString() + "\n" + currentState.Name;

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
}