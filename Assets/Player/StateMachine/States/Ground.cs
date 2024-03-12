using Godot;

[GlobalClass]
public partial class Ground : State
{
	public override void StateEnter()
	{
		machine.landSoundPlayer.Play(); // play landing sound
		machine.animPlayer.Play("Land");

		// after getting to the ground, recover the ability of jump and dash
		machine.canJump = true;
		machine.canDash = true;
	}

	public override void StatePhysicsProcess(double delta)
	{
		// movement
		machine.ApplyVerticalMovement(delta);
		machine.ApplyGravity(delta);

		// air tran
		if(!machine.player.IsOnFloor())
			EmitSignal(machine.TRANSITION_STRING, this, "air");
	}
}