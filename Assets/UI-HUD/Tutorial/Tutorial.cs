using Godot;

public partial class Tutorial : Control
{
	private Label moveLabel, jumpLabel, dashLabel;

	public override void _Ready()
	{
		moveLabel = (Label)GetChild(0);
		jumpLabel = (Label)GetChild(1);
		dashLabel = (Label)GetChild(2);

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