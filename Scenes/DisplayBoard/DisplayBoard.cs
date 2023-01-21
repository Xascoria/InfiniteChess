using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;

public class DisplayBoard : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	private BoardBG BoardBG;
	private Camera2D Camera;
	private Node2D PieceRoot;
	private const String LIGHT = "Light";
	private const String DARK = "Dark";
	private Dictionary<int, Sprite> piece_imgs_dict = new Dictionary<int, Sprite>();
	private Board myBoard = new Board(true);

	public override void _Ready()
	{
		BoardBG = GetNode<BoardBG>("BoardBG");
		Camera = GetNode<Camera2D>("Camera");
		PieceRoot = GetNode<Node2D>("PieceRoot");
		BoardBG.Connect(nameof(BoardBG.BoardClickedSig), this, nameof(BoardClicked));

		//TODO FILL THIS IN
		SetupBoard(myBoard);
	}

	private void SetupBoard(Board board) {
		for (int i = 0; i < 100; i += 1) {
			for (int j = 0; j < 100; j += 1) {
				int tile = BoardBG.TileSet.FindTileByName(i%2 == j%2 ? LIGHT : DARK);
				BoardBG.SetCell(i - 54, j - 54, tile);
			}
		}

		Camera.LimitLeft = -54 * 64;
		Camera.LimitRight = 46 * 64;
		Camera.LimitTop = -54 * 64;
		Camera.LimitBottom = 46 * 64;

		foreach (var piece in board.get_current_board()){
			String img_link = getImageLink(piece.Value.GetColor(), piece.Value.GetPieceType());
			Sprite sprite = new Sprite();

			sprite.Texture = GD.Load<Texture>(img_link);
			sprite.Scale = new Godot.Vector2(1.3f,1.3f);

			PieceRoot.AddChild(sprite);
			sprite.Position = new Godot.Vector2((float) piece.Key.Item2 * 64 + 32, (float) piece.Key.Item1 * 64 + 32);
			piece_imgs_dict.Add(piece.Value.GetIndex(), sprite);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsKeyPressed((int) KeyList.W)) {
			MoveCamera( Camera.Position.x, Camera.Position.y - 10);
		}
		if (Input.IsKeyPressed((int) KeyList.S)) {
			MoveCamera( Camera.Position.x, Camera.Position.y + 10);
		}
		if (Input.IsKeyPressed((int) KeyList.A)) {
			MoveCamera( Camera.Position.x - 10, Camera.Position.y);
		}
		if (Input.IsKeyPressed((int) KeyList.D)) {
			MoveCamera( Camera.Position.x + 10, Camera.Position.y);
		}
	}

	private void MoveCamera(float newx, float newy){
		newx = Math.Min(Math.Max(newx, Camera.LimitLeft + 640), Camera.LimitRight - 640);
		newy = Math.Min(Math.Max(newy, Camera.LimitTop + 360), Camera.LimitBottom - 360);

		Camera.Position = new Godot.Vector2(newx, newy);
	}

	// public override void _Input(InputEvent inputEvent)
	// {
	// 	if (inputEvent is InputEventMouseButton mouseEvent)
	// 	{
	// 		if (mouseEvent.IsPressed()){
	// 			GD.Print("mouse button event at ", 
	// 			BoardBG.WorldToMap((mouseEvent.GlobalPosition + Camera.GlobalPosition - new Godot.Vector2(640, 360))/4));
	// 		}
	// 	}
	// }

	public void BoardClicked(int x, int y){
		// myBoard.execute_move(
		// 	Tuple.Create<BigInteger, BigInteger>(new BigInteger(0), new BigInteger(0)),
		// 	Tuple.Create<BigInteger, BigInteger>(new BigInteger(y), new BigInteger(x))
		// 	);

		var t = Tuple.Create<BigInteger, BigInteger>(new BigInteger(y), new BigInteger(x));
		if (myBoard.get_current_board().ContainsKey(t)){
			GD.Print(myBoard.get_current_board()[t].GetName());
		}
		UpdateBoard(myBoard);
		GD.Print(x, " ", y);
	}

	public void UpdateBoard(Board board){
		foreach (var piece in board.get_current_board()){

			// if (piece.Key.Item1)
			// piece_imgs_dict[piece.Value.GetIndex()].Visible = true; 
			var x_val = piece.Key.Item1;
			var y_val = piece.Key.Item2;

			//-54 <= x_val <= 45 and -54 <= y_val <= 45
			var sprite = piece_imgs_dict[piece.Value.GetIndex()];
			if (-54 <= x_val && x_val <= 45 && -54 <= y_val && y_val <= 45){
				sprite.Visible = true; 
				sprite.Position = new Godot.Vector2((float) piece.Key.Item2 * 64 + 32, (float) piece.Key.Item1 * 64 + 32);
			} else {
				sprite.Visible = false;
			}
		}
	}

	private String getImageLink(Piece.PieceColor color, Piece.PieceType type) {
		String output = "res://Resources/Pieces/";
		if (color == Piece.PieceColor.White){
			output += "w";
		} else {
			output += "b";
		}

		if (type == Piece.PieceType.King){
			output += "K";
		} else if (type == Piece.PieceType.Queen){
			output += "Q";
		} else if (type == Piece.PieceType.Knight){
			output += "N";
		} else if (type == Piece.PieceType.Rook){
			output += "R";
		} else if (type == Piece.PieceType.Bishop){
			output += "B";
		} else {
			output += "P";
		}
		return output + ".svg";
	}
}
