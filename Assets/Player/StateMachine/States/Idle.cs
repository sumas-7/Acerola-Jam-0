using Godot;

[GlobalClass]
public partial class Idle : State
{
	public override void StatePhysicsProcess(double delta)
	{
		// movement
		ApplyVerticalMovement(delta);
		ApplyGravity(delta);

		// transitions
		JumpTransition();
		// move tran
        if(machine.inputDir.Length() > 0)
            EmitSignal(machine.TRANSITION_STRING, this, "move");
	}
}