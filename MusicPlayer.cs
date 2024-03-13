using Godot;

public partial class MusicPlayer : AudioStreamPlayer
{
	private string streamName;
	private AudioStream start, initialLoop, drop, mainLoop;

	public override void _Ready()
	{
		start = (AudioStream)GD.Load("res://Assets/Music/Start.ogg");
		initialLoop = (AudioStream)GD.Load("res://Assets/Music/InitialLoop.ogg");
		drop = (AudioStream)GD.Load("res://Assets/Music/Drop.ogg");
		mainLoop = (AudioStream)GD.Load("res://Assets/Music/MainLoop.ogg");

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
	}
}