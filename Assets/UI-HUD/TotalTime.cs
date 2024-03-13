using Godot;

public partial class TotalTime : RichTextLabel
{
	public override void _Ready()
	{
		Text += Mathf.Snapped(GameManager.Instance.speedrunTimer, 0.001f) + "s!";
	}
}