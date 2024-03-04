using Godot;

public partial class NextLevelButton : Button
{
    public override void _Ready()
    {
        Pressed += OnPress;
    }
    private void OnPress()
	{
        string currentLevelIndex = GetTree().CurrentScene.Name;
        int nextLevelIndex = currentLevelIndex.ToInt() + 1;

		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Assets/Levels/Level" + nextLevelIndex + ".tscn");
	}
}