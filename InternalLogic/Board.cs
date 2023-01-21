using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Collections;

public class Board 
{
    private bool has_promotion = true;
    private bool white_left_cast_possible = true;
    private bool white_right_cast_possible = true;
    private bool black_left_cast_possible = true;
    private bool black_right_cast_possible = true;

    private List<Move> moves_stack = new List<Move>();

    /*
    0,0 is the bottom left corner (A1 White rook)
    */
    private Dictionary<Tuple<BigInteger,BigInteger>, Piece> current_board = 
        new Dictionary<Tuple<BigInteger,BigInteger>, Piece>(){
            //White
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(0),new BigInteger(0)), 
            new Piece(Piece.PieceType.Rook, "King-side Rook", Piece.PieceColor.White, 1)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(0),new BigInteger(1)), 
            new Piece(Piece.PieceType.Knight, "King-side Knight", Piece.PieceColor.White, 2)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(0),new BigInteger(2)), 
            new Piece(Piece.PieceType.Bishop, "King-side Bishop", Piece.PieceColor.White, 3)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(0),new BigInteger(3)), 
            new Piece(Piece.PieceType.King, "King", Piece.PieceColor.White, 4)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(0),new BigInteger(4)), 
            new Piece(Piece.PieceType.Queen, "Queen", Piece.PieceColor.White, 5)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(0),new BigInteger(5)), 
            new Piece(Piece.PieceType.Bishop, "Queen-side Bishop", Piece.PieceColor.White, 6)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(0),new BigInteger(6)), 
            new Piece(Piece.PieceType.Knight, "Queen-side Knight", Piece.PieceColor.White, 7)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(0),new BigInteger(7)), 
            new Piece(Piece.PieceType.Rook, "Queen-side Rook", Piece.PieceColor.White, 8)},

            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(1),new BigInteger(0)), 
            new Piece(Piece.PieceType.Pawn, "H Pawn", Piece.PieceColor.White, 9)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(1),new BigInteger(1)), 
            new Piece(Piece.PieceType.Pawn, "G Pawn", Piece.PieceColor.White, 10)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(1),new BigInteger(2)), 
            new Piece(Piece.PieceType.Pawn, "F Pawn", Piece.PieceColor.White, 11)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(1),new BigInteger(3)), 
            new Piece(Piece.PieceType.Pawn, "E Pawn", Piece.PieceColor.White, 12)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(1),new BigInteger(4)), 
            new Piece(Piece.PieceType.Pawn, "D Pawn", Piece.PieceColor.White, 13)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(1),new BigInteger(5)), 
            new Piece(Piece.PieceType.Pawn, "C Pawn", Piece.PieceColor.White, 14)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(1),new BigInteger(6)), 
            new Piece(Piece.PieceType.Pawn, "B Pawn", Piece.PieceColor.White, 15)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(1),new BigInteger(7)), 
            new Piece(Piece.PieceType.Pawn, "A Pawn", Piece.PieceColor.White, 16)},

            //Black
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(7),new BigInteger(0)), 
            new Piece(Piece.PieceType.Rook, "King-side Rook", Piece.PieceColor.Black, 17)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(7),new BigInteger(1)), 
            new Piece(Piece.PieceType.Knight, "King-side Knight", Piece.PieceColor.Black, 18)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(7),new BigInteger(2)), 
            new Piece(Piece.PieceType.Bishop, "King-side Bishop", Piece.PieceColor.Black, 19)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(7),new BigInteger(3)), 
            new Piece(Piece.PieceType.King, "King", Piece.PieceColor.Black, 20)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(7),new BigInteger(4)), 
            new Piece(Piece.PieceType.Queen, "Queen", Piece.PieceColor.Black, 21)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(7),new BigInteger(5)), 
            new Piece(Piece.PieceType.Bishop, "Queen-side Bishop", Piece.PieceColor.Black, 22)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(7),new BigInteger(6)), 
            new Piece(Piece.PieceType.Knight, "Queen-side Knight", Piece.PieceColor.Black, 23)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(7),new BigInteger(7)), 
            new Piece(Piece.PieceType.Rook, "Queen-side Rook", Piece.PieceColor.Black, 24)},

            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(6),new BigInteger(0)), 
            new Piece(Piece.PieceType.Pawn, "H Pawn", Piece.PieceColor.Black, 25)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(6),new BigInteger(1)), 
            new Piece(Piece.PieceType.Pawn, "G Pawn", Piece.PieceColor.Black, 26)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(6),new BigInteger(2)), 
            new Piece(Piece.PieceType.Pawn, "F Pawn", Piece.PieceColor.Black, 27)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(6),new BigInteger(3)), 
            new Piece(Piece.PieceType.Pawn, "E Pawn", Piece.PieceColor.Black, 28)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(6),new BigInteger(4)), 
            new Piece(Piece.PieceType.Pawn, "D Pawn", Piece.PieceColor.Black, 29)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(6),new BigInteger(5)), 
            new Piece(Piece.PieceType.Pawn, "C Pawn", Piece.PieceColor.Black, 30)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(6),new BigInteger(6)), 
            new Piece(Piece.PieceType.Pawn, "B Pawn", Piece.PieceColor.Black, 31)},
            {Tuple.Create<BigInteger,BigInteger>(new BigInteger(6),new BigInteger(7)), 
            new Piece(Piece.PieceType.Pawn, "A Pawn", Piece.PieceColor.Black, 32)},
        };
    
    //Constructor
    public Board(bool has_promotion){
        this.has_promotion = has_promotion;
    }

    //TODO
    public bool move_is_legal(Tuple<BigInteger,BigInteger> from, Tuple<BigInteger,BigInteger> to){
        //Empty piece
        if (!current_board.ContainsKey(from)){
            return false;
        }

        //Wrong Starting Color
        if (getWhosTurn() != current_board[from].GetColor()){
            return false;
        }

        //Wrong Ending Color
        if (current_board.ContainsKey(to) && getWhosTurn() == current_board[to].GetColor()){
            return false;
        }

        //Same square
        if (from == to){
            return false;
        }

        var moved_piece = current_board[from];
        switch(moved_piece.GetPieceType())
        {
            case Piece.PieceType.King:
                if (square_is_under_attacked(to)){
                    return false;
                }

                break;
            case Piece.PieceType.Queen:
                break;
            case Piece.PieceType.Rook:
                break;
            case Piece.PieceType.Knight:
                break;
            case Piece.PieceType.Bishop:
                break;
            case Piece.PieceType.Pawn:
                break;
        }

        return true;
    }

    //TODO
    public void execute_move(Tuple<BigInteger,BigInteger> from, Tuple<BigInteger,BigInteger> to){
        var current_piece = current_board[from];

        switch(current_piece.GetPieceType())
        {
            case Piece.PieceType.King:
                break;
            case Piece.PieceType.Queen:
                break;
            case Piece.PieceType.Rook:
                break;
            case Piece.PieceType.Knight:
                break;
            case Piece.PieceType.Bishop:
                break;
            case Piece.PieceType.Pawn:
                break;
        }
        current_board.Remove(from);
        current_board[to] = current_piece;

        moves_stack.Add(new Move());
    }

    public bool undo_move(){
        if (moves_stack.Count == 0){
            return false;
        }

        //TODO
        return true;

    }

    //TODO
    public bool is_in_check(){
        return false;
    }

    //TODO
    public bool is_in_checkmate(){
        return false;
    }

    //TODO
    public bool square_is_under_attacked(Tuple<BigInteger, BigInteger> coord){
        return false;
    }

    public Dictionary<Tuple<BigInteger,BigInteger>, Piece> get_current_board(){
        return current_board;
    }

    private Piece.PieceColor getWhosTurn(){
        if (moves_stack.Count % 2 == 0){
            return Piece.PieceColor.White;
        } 
        return Piece.PieceColor.Black;
    }
    
    //TODO
    private bool AllowEnPassant(){
        if (moves_stack.Count == 0){
            return false;
        }
        return true;
    }

}
