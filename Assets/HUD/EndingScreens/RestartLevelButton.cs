using Godot;

public partial class RestartLevelButton : Button
{
    public override void _Ready()
    {
        Pressed += OnPress;
    }
    private void OnPress()
	{
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}
}