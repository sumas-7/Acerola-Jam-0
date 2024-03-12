using Godot;

public partial class AudioVisualAberrator : Area2D
{
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered; // connects the built in signal to the function
	}

    private void OnBodyEntered(Node2D body)
	{
		if(body.Name == "Player")
		{
			GameManager.Instance.InvertColors();
			this.QueueFree();
		}
	}
}