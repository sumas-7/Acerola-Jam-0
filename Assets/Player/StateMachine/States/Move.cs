using Godot;

[GlobalClass]
public partial class Move : State
{
	public override void StatePhysicsProcess(double delta)
	{
		// movement
		ApplyVerticalMovement(delta);
		ApplyGravity(delta);

		// transitions
		JumpTransition();
		// idle tran
        if(machine.inputDir.Length() == 0)
            EmitSignal(machine.TRANSITION_STRING, this, "idle");
		if(Input.IsActionJustPressed("dash"))
			EmitSignal(machine.TRANSITION_STRING, this, "dash");
	}
}