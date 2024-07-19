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
        GetParentOrNull<Control>().Visible = false; // sets the level ending screen to invisible again
		GameManager.Instance.LoadLevel(loadNext);
	}
}