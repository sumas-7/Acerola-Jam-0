using Godot;

public partial class Prop : Area2D
{
	[Export] private PackedScene spawnatedScreen_Scene;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered; // connects the built in signal to the function
	}

    private void OnBodyEntered(Node2D body)
	{
		if(body.Name == "Player") // if collides with the player
		{
			GetTree().Paused = true; // stops the game

			// spawns the selected screen
			Control spawnatedScreen = (Control)spawnatedScreen_Scene.Instantiate();
			GetOwner<Node2D>().GetChild(0).AddChild(spawnatedScreen);
		}
	}
}