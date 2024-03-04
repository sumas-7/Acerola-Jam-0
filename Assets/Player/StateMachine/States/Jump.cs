using Godot;

[GlobalClass]
public partial class Jump : State
{
	public override void StatePhysicsProcess(double delta)
	{
		// movement
		ApplyVerticalMovement(delta);
		ApplyGravity(delta);

		// idle tran
        if(machine.player.IsOnFloor())
			if(machine.inputDir.Length() > 0)
				EmitSignal(machine.TRANSITION_STRING, this, "move");
			else
				EmitSignal(machine.TRANSITION_STRING, this, "idle");
		
		if(Input.IsActionJustPressed("dash"))
			EmitSignal(machine.TRANSITION_STRING, this, "dash");
	}
}