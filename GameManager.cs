using System.Collections.Generic;
using Godot;

public partial class GameManager : Node2D
{
	public static GameManager Instance;

	private int levelIndex = 0;
	private int lastLevelIndex = 2;
	private Node2D level;
	private Control tutorial;
	private PackedScene level_scene = (PackedScene)GD.Load("res://Assets/Levels/Level0.tscn");
	private PackedScene tutorial_scene = (PackedScene)GD.Load("res://Assets/UI-HUD/Tutorial/Tutorial.tscn");

	private Node levelControls; // node in the level that contains control scheme
	// create the objects of the InputEventKeys. Using only one object ended up assigning the same key to everything
	private InputEventKey upKey = new(), downKey = new(), leftKey = new(), rightKey = new(), jumpKey = new(), dashKey = new();
	public List<Key> currentControls = new();

	public override void _Ready()
	{
		Instance = this;
	}

	public void LoadLevel(bool loadNext)
	{
		GetChild(0).QueueFree();

		if(loadNext) // if true, loads the next level in order
			level_scene = (PackedScene)GD.Load("res://Assets/Levels/Level" + ++levelIndex + ".tscn");

		// instantiate the new level
		level = (Node2D)level_scene.Instantiate();
		AddChild(level);

		// get the controls of the level and change the InputMap
		levelControls = level.GetChild(1);
		AberrateInput();

		// add the tutorial scene to the level
		tutorial = (Control)tutorial_scene.Instantiate();
		level.AddChild(tutorial);	

		GetTree().Paused = false; // resume the game
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
			Key key; // needs to also add the key on the list

			key = (Key)levelControls.GetChild(0).Name.ToString()[0];
			currentControls.Add(key);
			upKey.Keycode = key;

			key = (Key)levelControls.GetChild(1).Name.ToString()[0];
			currentControls.Add(key);
			downKey.Keycode = key;

			key = (Key)levelControls.GetChild(2).Name.ToString()[0];
			currentControls.Add(key);
			leftKey.Keycode = key;

			key = (Key)levelControls.GetChild(3).Name.ToString()[0];
			currentControls.Add(key);
			rightKey.Keycode = key;
			

			key = (Key)levelControls.GetChild(4).Name.ToString()[0];
			currentControls.Add(key);
			jumpKey.Keycode = key;

			key = (Key)levelControls.GetChild(5).Name.ToString()[0];
			currentControls.Add(key);
			dashKey.Keycode = key;
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

		// prints current control scheme
		GD.Print
		(
			InputMap.ActionGetEvents("up") + "\n" +
			InputMap.ActionGetEvents("down") + "\n" +
			InputMap.ActionGetEvents("left") + "\n" +
			InputMap.ActionGetEvents("right") + "\n" +
			InputMap.ActionGetEvents("jump") + "\n" +
			InputMap.ActionGetEvents("dash") + "\n"
		);
		
		foreach(int key in currentControls)
		{
			GD.Print((Key)key);
		}
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