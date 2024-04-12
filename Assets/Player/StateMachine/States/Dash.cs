using Godot;

[GlobalClass]
public partial class Dash : State
{
    private float dashTimer = 0;
    private float changeDirTimer = 0;

	public override void StateEnter()
	{
		machine.dashSoundPlayer.Play();
		dashTimer = machine.DASH_DURATION;
		changeDirTimer = machine.DASH_DIR_CHANGE_TIME;
		machine.canDash = false;
	}

    public override void StateExit()
    {
        machine.velocity.Y = 0;
    }
    public override void StatePhysicsProcess(double delta)
	{
		// movement
        machine.velocity = machine.lastDir.Normalized() * machine.DASH_SPEED; // dashes to the last direction you were "facing" before pressing dash

		// this allows to change direction after you've already dashed (only works when pressing dash + newDir at the same time)		
		if(changeDirTimer > 0 && machine.inputDir != machine.lastDir && machine.inputDir != Vector2.Zero)
		{
			machine.lastDir = machine.inputDir; // changes the direction of the dash to be your current input dir
			changeDirTimer = 0; // resets the timer to avoid changing dir more than once
		}

		changeDirTimer -= (float)delta;

		// transitions
		if(machine.player.IsOnFloor())
            EmitSignal(machine.TRANSITION_STRING, this, "air"); // stop dashing

		if(dashTimer <= 0)

		{
			dashTimer = machine.DASH_DURATION; // resets timer
            EmitSignal(machine.TRANSITION_STRING, this, "air"); // stop dashing
		}
		else
			dashTimer -= (float)delta;
	}
}