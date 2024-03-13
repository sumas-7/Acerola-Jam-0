using Godot;

public partial class MusicPlayer : AudioStreamPlayer
{
	private string streamName;
	private AudioStream start, initialLoop, drop, mainLoop;

	public override void _Ready()
	{
		start = (AudioStream)GD.Load("res://Assets/Music/Start.wav");
		initialLoop = (AudioStream)GD.Load("res://Assets/Music/InitialLoop.wav");
		drop = (AudioStream)GD.Load("res://Assets/Music/Drop.wav");
		mainLoop = (AudioStream)GD.Load("res://Assets/Music/MainLoop.wav");

		Stream = start;
		Play();
	}

	public override void _Process(double delta)
	{
		streamName = Stream.ResourcePath.GetFile().GetBaseName();
		if(GetPlaybackPosition() == 0)
		{
			switch(streamName)
			{
				case "Start":
					Stream = initialLoop;
					Play();
				break;

				case "InitialLoop":
					if(GameManager.Instance.levelIndex != 0)
						Stream = drop;

					Play();
				break;

				default:
					Stream = mainLoop;
					Play();
				break;
			}
		}
		GD.Print(streamName);
	}
}