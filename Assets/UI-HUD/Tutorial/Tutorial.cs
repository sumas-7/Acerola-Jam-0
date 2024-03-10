using Godot;

public partial class Tutorial : Control
{
	private RichTextLabel moveLabel, jumpLabel, dashLabel;

	public override void _Ready()
	{
		moveLabel = (RichTextLabel)GetChild(0);
		jumpLabel = (RichTextLabel)GetChild(1);
		dashLabel = (RichTextLabel)GetChild(2);

		moveLabel.Text += 
			GameManager.Instance.currentControls[0].ToString() + " " +
			GameManager.Instance.currentControls[2].ToString() + " " +
			GameManager.Instance.currentControls[1].ToString() + " " +
			GameManager.Instance.currentControls[3].ToString();
			
		jumpLabel.Text += GameManager.Instance.currentControls[4].ToString();
		dashLabel.Text += GameManager.Instance.currentControls[5].ToString();
	}

	public override void _Process(double delta)
	{
	}
}