using Godot;

public partial class State : Node
{
	[Signal]
	public delegate void TransitionEventHandler(State oldState, string newStateName); // the signal emitted when transitioning to a new state

	public StateMachine machine;

    public override void _Ready()
    {
        machine = GetOwner<StateMachine>();
    }

	public virtual void StateEnter(){} // function called when you first enter this state
	public virtual void StateExit(){} // function called when you leave the state
	public virtual void StateProcess(double delta){} // the state's process function
	public virtual void StatePhysicsProcess(double delta){} // the state's PhysicsProcess function

	// function to call when the state includes moving
	public void ApplyVerticalMovement(double delta)
    {
		machine.velocity.X = machine.inputDir.X * machine.speed;
    }
	// function to call when the state includes gravity
    public void ApplyGravity(double delta)
    {
        if(!machine.player.IsOnFloor())
			machine.velocity.Y += machine.gravity * (float)delta;
    }

	public virtual void JumpTransition()
    {
        if(Input.IsActionJustPressed("jump")) // if you press jump
        {
			machine.velocity.Y = -machine.JumpStrength; // changes Y velocity making the player jump
			EmitSignal(machine.TRANSITION_STRING, this, "jump"); // changes state to jump
        }
    }
}