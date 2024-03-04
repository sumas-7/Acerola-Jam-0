using Godot;

public partial class EndGoal : Area2D
{
	private PackedScene levelClearScreen_Scene;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered; // connects the built in signal to the function
		levelClearScreen_Scene = (PackedScene)GD.Load("res://Assets/HUD/LevelClearScreen/LevelClearScreen.tscn");
	}

    private void OnBodyEntered(Node2D body)
	{
		if(body.Name == "Player")
		{
			GetTree().Paused = true; // stops the game

			// spawns EndLevelScreen
			Control levelClearSreen = (Control)levelClearScreen_Scene.Instantiate();
			GetOwner<Node2D>().GetChild(0).AddChild(levelClearSreen);
		}
	}
}