using Godot;

[GlobalClass]
public partial class Ground : State
{
	public override void StateEnter()
	{
		machine.canDash = true;
	}

	public override void StatePhysicsProcess(double delta)
	{
		// movement
		machine.ApplyVerticalMovement(delta);
		machine.ApplyGravity(delta);

		// transitions
		if(Input.IsActionJustPressed("jump"))
		{
			
		}
        if(Input.IsActionJustPressed("jump") && machine.player.IsOnFloor()) // if you press jump
        {
			machine.velocity.Y = -machine.JUMP_STRENGTH; // changes Y velocity making the player jump
			EmitSignal(machine.TRANSITION_STRING, this, "air"); // changes state to jump
        }
	}
}