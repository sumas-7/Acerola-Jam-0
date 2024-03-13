using Godot;

public partial class MusicButton : Button
{
	public override void _Ready()
    {
        Pressed += OnPress;
    }
    private void OnPress()
	{
        GameManager.Instance.ToggleMusic();
	}
}