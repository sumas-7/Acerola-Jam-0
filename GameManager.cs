using Godot;

public partial class GameManager : Node2D
{
	public static GameManager Instance;

	private int levelIndex = 0;
	private int lastLevelIndex = 2;
	private Node2D level;
	private PackedScene level_scene = (PackedScene)GD.Load("res://Assets/Levels/Level0.tscn");

	public override void _Ready()
	{
		Instance = this;
	}

	public void LoadLevel(bool loadNext)
	{
		GetChild(0).QueueFree();

		if(loadNext) // if true, loads the next level in order
			level_scene = (PackedScene)GD.Load("res://Assets/Levels/Level" + ++levelIndex + ".tscn");

		if(levelIndex <= lastLevelIndex)
		{
			level = (Node2D)level_scene.Instantiate();
			AddChild(level);
		}

		GetTree().Paused = false;
	}
}