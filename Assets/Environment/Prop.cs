using Godot;

public partial class Prop : Area2D
{
	[Export] private bool areYaWinningSon;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered; // connects the built in signal to the function
	}

    private void OnBodyEntered(Node2D body)
	{
		if(body.Name == "Player") // if collides with the player
		{
			if(areYaWinningSon)
				GameManager.Instance.Win();
			else
				GameManager.Instance.Lose();
		}
	}
}