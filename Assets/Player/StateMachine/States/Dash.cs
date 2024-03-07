using Godot;

[GlobalClass]
public partial class Dash : State
{
    private float timer = 0.1f;

    public override void StateExit()
    {
        machine.velocity = Vector2.Zero;
    }
    public override void StatePhysicsProcess(double delta)
	{
		// movement
        machine.velocity = (machine.inputDir).Normalized() * 1200;
		
		// transitions
		if(timer <= 0)
		{
			timer = 0.1f; // resets timer
            EmitSignal(machine.TRANSITION_STRING, this, "idle"); // return to idle
		}
		else
			timer -= (float)delta;
			
	}
}