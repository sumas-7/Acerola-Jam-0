using Godot;

public partial class Tutorial : Control
{
	private Player player;
	private Label moveLabel, jumpLabel, dashLabel;

	public override void _Ready()
	{
		player = GetParent().GetChildOrNull<Player>(3);
		Position = player.Position + new Vector2(400, -300); // position in front of the player

		moveLabel = (Label)GetChild(0);
		jumpLabel = (Label)GetChild(1);
		dashLabel = (Label)GetChild(2);

		moveLabel.Text += 
			GameManager.Instance.currentControls[0].ToString() +
			GameManager.Instance.currentControls[1].ToString() +
			GameManager.Instance.currentControls[2].ToString() +
			GameManager.Instance.currentControls[3].ToString();
			
		jumpLabel.Text += GameManager.Instance.currentControls[4].ToString();
		dashLabel.Text += GameManager.Instance.currentControls[5].ToString();
	}

	public override void _Process(double delta)
	{
	}
}