using Godot;

public partial class DebugHUD : Control
{
	public static Label Instance;

	public override void _Ready()
	{
		Instance = (Label)GetChild(0);
	}
}
