using Godot;
using System;

public class BoardBG : TileMap
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.

	[Signal]
	public delegate void BoardClickedSig(int x, int y);

	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == (int) ButtonList.Left && mouseEvent.IsPressed()){
				var coord = this.WorldToMap((GetLocalMousePosition()));
				// GD.Print("mouse button event at ", coord);
				
				EmitSignal(nameof(BoardClickedSig), (int) coord.x, (int) coord.y);
			}
		}
	}
}
