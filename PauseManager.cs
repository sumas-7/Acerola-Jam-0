using Godot;

public partial class PauseManager : Node // pause manager needs to exist otherwise the MasterNode would get paused, soft locking the game
{
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("pause") && GameManager.Instance.isPlayerActive)
			GameManager.Instance.TogglePauseMenu();
	}
}