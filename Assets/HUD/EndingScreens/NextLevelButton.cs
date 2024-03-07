using Godot;

public partial class NextLevelButton : Button
{
    public override void _Ready()
    {
        Pressed += OnPress;
    }
    private void OnPress()
	{
        string currentLevelName = GetTree().CurrentScene.Name;
        int nextLevelIndex = currentLevelName.ToInt() + 1;

		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Assets/Levels/Level" + nextLevelIndex + ".tscn");
	}
}