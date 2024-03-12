using Godot;

public partial class BloomButton : Button
{
	public override void _Ready()
    {
        Pressed += OnPress;
    }
    private void OnPress()
	{
        GameManager.Instance.ToggleBloom();
	}
}