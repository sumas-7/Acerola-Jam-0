using Godot;

[GlobalClass]
public partial class Air : State
{
	public override void StatePhysicsProcess(double delta)
	{
		// movement
		machine.ApplyVerticalMovement(delta);
		machine.ApplyGravity(delta);

		// ground tran
		if(machine.player.IsOnFloor())
			EmitSignal(machine.TRANSITION_STRING, this, "ground");
		
		// dash tran
		if(Input.IsActionJustPressed("dash") && machine.canDash)
			EmitSignal(machine.TRANSITION_STRING, this, "dash");
	}
}