using Godot;

public partial class Tutorial : CanvasLayer
{
	private RichTextLabel moveLabel, upLabel, jumpLabel, dashLabel;

	public override void _Ready()
	{
		moveLabel = (RichTextLabel)GetChild(0);
		upLabel = (RichTextLabel)GetChild(1);
		jumpLabel = (RichTextLabel)GetChild(2);
		dashLabel = (RichTextLabel)GetChild(3);

		moveLabel.Text += 
			GameManager.Instance.currentControls[2].ToString() + " " + // left
			GameManager.Instance.currentControls[1].ToString() + " " + // down
			GameManager.Instance.currentControls[3].ToString(); // right
			
		upLabel.Text += GameManager.Instance.currentControls[0].ToString();

		jumpLabel.Text += GameManager.Instance.currentControls[4].ToString(); // jump
		dashLabel.Text += GameManager.Instance.currentControls[5].ToString(); // dash
	}

	public override void _Process(double delta)
	{
		if(GetTree().Paused)
			Visible = false;
		else
			Visible = true;
	}
}