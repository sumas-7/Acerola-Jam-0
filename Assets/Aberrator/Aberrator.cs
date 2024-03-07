using System.Collections.Generic;
using Godot;

public partial class Aberrator : Node
{
	// create the objects of the InputEventKeys. Using only one object ended up assigning the same key to everything
	InputEventKey upKey = new(), downKey = new(), leftKey = new(), rightKey = new(), jumpKey = new(), dashKey = new();
	List<Key> usedKeys = new();

    public override void _Ready()
	{
		// erases all events from all actions
		InputMap.ActionEraseEvents("up");
		InputMap.ActionEraseEvents("down");
		InputMap.ActionEraseEvents("left");
		InputMap.ActionEraseEvents("right");
		InputMap.ActionEraseEvents("jump");
		InputMap.ActionEraseEvents("dash");

		// for each inputEventKey object, assign a keycode to it
		upKey.Keycode = GetRandomKeyCode();
		downKey.Keycode = GetRandomKeyCode();
		leftKey.Keycode = GetRandomKeyCode();
		rightKey.Keycode = GetRandomKeyCode();
		
		jumpKey.Keycode = GetRandomKeyCode();
		dashKey.Keycode = GetRandomKeyCode();

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
		
		foreach(int key in usedKeys)
		{
			GD.Print((Key)key);
		}
	}

	private Key GetRandomKeyCode()
	{
		// from 65 to 90 = from A to Z
		// higher than 90 is a special case
		int num = GD.RandRange(65, 98);

		if(!usedKeys.Contains(GetKey(num))) // only add the key if it hasn't been already used
			switch(num)
			{
				case 91: usedKeys.Add(Key.Space); return Key.Space;
				case 92: usedKeys.Add(Key.Alt); return Key.Alt;
				case 93: usedKeys.Add(Key.Ctrl); return Key.Ctrl;
				case 94: usedKeys.Add(Key.Shift); return Key.Shift;
				case 95: usedKeys.Add(Key.Capslock); return Key.Capslock;
				case 96: usedKeys.Add(Key.Tab); return Key.Tab;
				case 97: usedKeys.Add(Key.Enter); return Key.Enter;
				case 98: usedKeys.Add(Key.Backspace); return Key.Backspace;
				default: usedKeys.Add((Key)num); return (Key)num;
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