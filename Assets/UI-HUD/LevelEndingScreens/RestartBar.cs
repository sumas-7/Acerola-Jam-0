using Godot;

public partial class RestartBar : ProgressBar
{
	private Control parent;

    public override void _Ready()
    {
        parent = (Control)GetParent();
    }
    public override void _Process(double delta)
	{
		if(Input.IsAnythingPressed() && parent.Visible == true)
		{
			Value += delta;
		}
		else
			Value -= delta;

		if(Value == MaxValue)
		{
			GetParentOrNull<Control>().Visible = false; // sets the screen to invisible again
			GameManager.Instance.LoadLevel(false);
		}
	}
}