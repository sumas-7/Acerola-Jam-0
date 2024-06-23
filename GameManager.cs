using System.Collections.Generic;
using Godot;

public partial class GameManager : Node2D
{
	public static GameManager Instance;

	public int levelIndex = 0;
	public float speedrunTimer = 0;
	public bool isPlayerActive = false;
	public Panel creditsScreen;
	private Node2D level;
	private CharacterBody2D player;
	private CanvasLayer hud;
	private CanvasLayer tutorial;
	private Control clearScreen, failScreen;
	private Panel settingsMenu, pauseMenu;
	private ShaderMaterial postProcessShader;
	private WorldEnvironment worldEnvironment;
	private Environment environment;
	private PackedScene level_scene;
	private PackedScene tutorial_scene = (PackedScene)GD.Load("res://Assets/UI-HUD/Tutorial/Tutorial.tscn");

	private Node levelControls; // node in the level that contains control scheme
	// create the objects of the InputEventKeys. Using only one object ended up assigning the same key to everything
	private InputEventKey upKey = new(), downKey = new(), leftKey = new(), rightKey = new(), jumpKey = new(), dashKey = new();
	public List<Key> currentControls = new();

	public override void _Ready()
	{
		Instance = this;
		worldEnvironment = (WorldEnvironment)GetChild(0);
		environment = worldEnvironment.Environment;

		hud = (CanvasLayer)worldEnvironment.GetChild(0);
		clearScreen = (Control)hud.GetChild(0);
		failScreen = (Control)hud.GetChild(1);
		pauseMenu = (Panel)hud.GetChild(3);
		settingsMenu = (Panel)hud.GetChild(4);
		creditsScreen = (Panel)hud.GetChild(5);

		ColorRect PostProcessRect = (ColorRect)hud.GetChild(2);
		postProcessShader = (ShaderMaterial)PostProcessRect.Material;
	}
    public override void _Process(double delta)
    {
		if(levelIndex != 0 && isPlayerActive)
        	speedrunTimer += (float)delta;
	}

    public void LoadLevel(bool loadNext)
	{
		GetChild(1).QueueFree();

		if(loadNext) // if true, loads the next level in order
			level_scene = (PackedScene)GD.Load("res://Assets/Levels/Level" + ++levelIndex + ".tscn");

		// instantiate the new level
		level = (Node2D)level_scene.Instantiate();
		AddChild(level);

		if(levelIndex < 6)
			player = (CharacterBody2D)level.GetChild(2);

		// get the controls of the level and change the InputMap
		levelControls = level.GetChild(0);
		
		if(loadNext)
			AberrateInput();

		if(level.Name != "EndingScreen") // if not the last level
		{
			// add the tutorial scene to the level
			tutorial = (CanvasLayer)tutorial_scene.Instantiate();
			level.AddChild(tutorial);

			isPlayerActive = true;
		}
		else
		{
			MusicPlayer musicPlayer = (MusicPlayer)worldEnvironment.GetChild(3);
			
			postProcessShader.SetShaderParameter("invert", false); // return to normal colors
			musicPlayer.PlayEnding();

			// connects the quit button in the last level to the quit method
			Button quitButton = (Button)level.GetChild(0).GetChild(2);
			quitButton.Pressed += Quit;
			
			// connects the credits text in the last level to the ToggleCredits method
			LinkButton creditsButton = (LinkButton)level.GetChild(1);
			creditsButton.Pressed += ToggleCredits;
		}


		GetTree().Paused = false; // resume the game
	}

	public void Win()
	{
		player.Visible = false;
		player.GetChild(1).QueueFree(); // destroys the StateMachine

		AudioStreamPlayer audioPlayer = (AudioStreamPlayer)worldEnvironment.GetChild(2); // play the WIN sound
		audioPlayer.Play();

		isPlayerActive = false;
		clearScreen.Visible = true;
	}
	public void Lose()
	{
		player.Visible = false;
		player.GetChildOrNull<CollisionShape2D>(0).QueueFree(); // destroys the Collision to avoid colliding again
		player.GetChildOrNull<StateMachine>(1).QueueFree(); // destroys the StateMachine

		AudioStreamPlayer audioPlayer = (AudioStreamPlayer)worldEnvironment.GetChild(1); // play the LOSE sound
		audioPlayer.Play();

		isPlayerActive = false;
		failScreen.Visible = true;
	}
	public void Quit()
	{
		GetTree().Quit();
	}

	// settings
	public void TogglePauseMenu() // shitty code
	{
		if(settingsMenu.Visible)
		{
			ToggleSettings();
		}
		else
		{
			pauseMenu.Visible = !pauseMenu.Visible;
			GetTree().Paused = !GetTree().Paused;
		}

		postProcessShader.SetShaderParameter("darken", GetTree().Paused);
	}
	public void ToggleSettings()
	{
		settingsMenu.Visible = !settingsMenu.Visible;
	}
	public void ApplySettings()
	{
		CheckBox fullscreen = (CheckBox)settingsMenu.GetChild(2).GetChild(0);
		CheckBox bloom = (CheckBox)settingsMenu.GetChild(2).GetChild(1);
		CheckBox music = (CheckBox)settingsMenu.GetChild(2).GetChild(2);

		// fullscreen
		if(fullscreen.ButtonPressed)
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
		else
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);

		// bloom
		environment.GlowEnabled = bloom.ButtonPressed;

		// music
		int musicBus = AudioServer.GetBusIndex("Music");
		AudioServer.SetBusMute(musicBus, !music.ButtonPressed);
	}
	public void ToggleCredits()
	{
		creditsScreen.Visible = !creditsScreen.Visible;
	}

	// Aberrations
	public void InvertColors()
	{
		postProcessShader.SetShaderParameter("invert", true);
	}
	public void AberrateAudio()
	{
		// change the player's sfx
		StateMachine playerMachine = player.GetChild<StateMachine>(1);
		MusicPlayer musicPlayer = (MusicPlayer)worldEnvironment.GetChild(3);

		playerMachine.jumpSoundPlayer.Stream = playerMachine.aberrantJump;
		playerMachine.dashSoundPlayer.Stream = playerMachine.aberrantDash;

		musicPlayer.Aberrate();
	}

	public void AberrateInput()
	{
		// clears current control scheme list
		currentControls.Clear();

		// erases all events from all actions
		InputMap.ActionEraseEvents("up");
		InputMap.ActionEraseEvents("down");
		InputMap.ActionEraseEvents("left");
		InputMap.ActionEraseEvents("right");
		InputMap.ActionEraseEvents("jump");
		InputMap.ActionEraseEvents("dash");

		// for each inputEventKey object, assign a keycode to it
		if(levelControls.GetChildOrNull<Node>(5) != null) // if there are specific controls for the level use those
		{
			int keyNum;
			string keyString; // needs to also add the key on the list

			keyString = levelControls.GetChild(0).Name; // gets the name of the node
			keyNum = keyString.ToInt(); // converts it to a int
			currentControls.Add((Key)keyNum); // adds it to the current controls list as a key
			upKey.Keycode = (Key)keyNum; // sets the key variable

			keyString = levelControls.GetChild(1).Name;
			keyNum = keyString.ToInt();
			currentControls.Add((Key)keyNum);
			downKey.Keycode = (Key)keyNum;

			keyString = levelControls.GetChild(2).Name;
			keyNum = keyString.ToInt();
			currentControls.Add((Key)keyNum);
			leftKey.Keycode = (Key)keyNum;

			keyString = levelControls.GetChild(3).Name;
			keyNum = keyString.ToInt();
			currentControls.Add((Key)keyNum);
			rightKey.Keycode = (Key)keyNum;
			

			keyString = levelControls.GetChild(4).Name;
			keyNum = keyString.ToInt();
			currentControls.Add((Key)keyNum);
			jumpKey.Keycode = (Key)keyNum;

			keyString = levelControls.GetChild(5).Name;
			keyNum = keyString.ToInt();
			currentControls.Add((Key)keyNum);
			dashKey.Keycode = (Key)keyNum;
		}
		else // else just get random ones
		{
			upKey.Keycode = GetRandomKeyCode();
			downKey.Keycode = GetRandomKeyCode();
			leftKey.Keycode = GetRandomKeyCode();
			rightKey.Keycode = GetRandomKeyCode();
			
			jumpKey.Keycode = GetRandomKeyCode();
			dashKey.Keycode = GetRandomKeyCode();
		}

		// add the events as actions of their desired type
		InputMap.ActionAddEvent("up", upKey);
		InputMap.ActionAddEvent("down", downKey);
		InputMap.ActionAddEvent("left", leftKey);
		InputMap.ActionAddEvent("right", rightKey);

		InputMap.ActionAddEvent("jump", jumpKey);
		InputMap.ActionAddEvent("dash", dashKey);

		// // prints current control scheme
		// GD.Print
		// (
		// 	InputMap.ActionGetEvents("up") + "\n" +
		// 	InputMap.ActionGetEvents("down") + "\n" +
		// 	InputMap.ActionGetEvents("left") + "\n" +
		// 	InputMap.ActionGetEvents("right") + "\n" +
		// 	InputMap.ActionGetEvents("jump") + "\n" +
		// 	InputMap.ActionGetEvents("dash") + "\n"
		// );
		
		// foreach(int key in currentControls)
		// {
		// 	GD.Print((Key)key);
		// }
	}
	private Key GetRandomKeyCode()
	{
		// from 65 to 90 = from A to Z
		// higher than 90 is a special case
		int num = GD.RandRange(65, 98);

		if(!currentControls.Contains(GetKey(num))) // only add the key if it hasn't been already used
			switch(num)
			{
				case 91: currentControls.Add(Key.Space); return Key.Space;
				case 92: currentControls.Add(Key.Alt); return Key.Alt;
				case 93: currentControls.Add(Key.Ctrl); return Key.Ctrl;
				case 94: currentControls.Add(Key.Shift); return Key.Shift;
				case 95: currentControls.Add(Key.Capslock); return Key.Capslock;
				case 96: currentControls.Add(Key.Tab); return Key.Tab;
				case 97: currentControls.Add(Key.Enter); return Key.Enter;
				case 98: currentControls.Add(Key.Backspace); return Key.Backspace;
				default: currentControls.Add((Key)num); return (Key)num;
			}
		else // else call itself again
			return GetRandomKeyCode();
	}
	// get a key based on a number so that i can choose a special key without having to use 4mil as number in the rnd num generator
	public Key GetKey(int num)
	{
        return num switch
        {
            91 => Key.Space,
            92 => Key.Alt,
            93 => Key.Ctrl,
            94 => Key.Shift,
            95 => Key.Capslock,
            96 => Key.Tab,
            97 => Key.Enter,
            98 => Key.Backspace,
            _ => (Key)num,
        };
    }
}