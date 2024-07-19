using Godot;

public partial class LoadLevelProgressBar : ProgressBar
{
    [Export] private bool loadNext;
	private Control parent;

    public override void _Ready()
    {
        parent = (Control)GetParent();
    }
    public override void _Process(double delta)
	{
		if(GetTree().Paused == false)
		{
			if(Input.IsAnythingPressed() && parent.Visible == true)
				Value += delta;
			else
				Value -= delta;

			if(Value == MaxValue)
			{
				GetParentOrNull<Control>().Visible = false; // sets the level ending screen to invisible again
				GameManager.Instance.LoadLevel(loadNext);
			}
		}
	}
}