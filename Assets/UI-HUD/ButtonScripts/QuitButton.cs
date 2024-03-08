using Godot;

public partial class QuitButton : Button
{
	public override void _Ready()
    {
        Pressed += OnPress;
    }
    private void OnPress()
	{
		GetTree().Quit();
	}
}
