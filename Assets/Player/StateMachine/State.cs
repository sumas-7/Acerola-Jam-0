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

	public virtual void AirTransition()
    {
        if(Input.IsActionJustPressed("jump") && machine.player.IsOnFloor()) // if you press jump
        {
			machine.velocity.Y = -machine.JUMP_STRENGTH; // changes Y velocity making the player jump
			EmitSignal(machine.TRANSITION_STRING, this, "air"); // changes state to jump
        }
    }
}