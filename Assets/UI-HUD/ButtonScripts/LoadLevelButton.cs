using Godot;

public partial class LoadLevelButton : Button
{
    [Export] private bool loadNext;

	public override void _Ready()
    {
        Pressed += OnPress;
    }
    private void OnPress()
	{
		GameManager.Instance.LoadLevel(loadNext);
        GetParentOrNull<Control>().Visible = false; // sets the screen to invisible again
	}
}