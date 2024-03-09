using Godot;

[GlobalClass]
public partial class Dash : State
{
    private float timer = 0;

	public override void StateEnter()
	{
		timer = machine.DASH_DURATION;
		machine.canDash = false;
	}

    public override void StateExit()
    {
        machine.velocity = Vector2.Zero;
    }
    public override void StatePhysicsProcess(double delta)
	{
		// movement
        machine.velocity = machine.lastDir.Normalized() * machine.DASH_SPEED;
		
		// transitions
		if(machine.player.IsOnFloor())
            EmitSignal(machine.TRANSITION_STRING, this, "air"); // stop dashing


		if(timer <= 0)
		{
			timer = machine.DASH_DURATION; // resets timer
            EmitSignal(machine.TRANSITION_STRING, this, "air"); // stop dashing
		}
		else
			timer -= (float)delta;
	}
}