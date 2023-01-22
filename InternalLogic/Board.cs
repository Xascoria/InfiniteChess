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

                //Castling

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
                //En Passant
                break;
        }

        return true;
    }

    //TODO
    public void execute_move(Tuple<BigInteger,BigInteger> from, Tuple<BigInteger,BigInteger> to, Piece.PieceType promotion_type){
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

    public bool is_in_check(){
        foreach (var pair in current_board){
            if (pair.Value.GetPieceType() == Piece.PieceType.King && pair.Value.GetColor() == getWhosTurn()){
                return square_is_under_attacked(pair.Key);
            }
        }

        return false;
    }

    //TODO
    public bool is_in_checkmate(){
        //Moving the King
        //Blocking
        //Take Out the Attacker

        return false;
    }

    //TODO
    public bool square_is_under_attacked(Tuple<BigInteger, BigInteger> coord){
        var opponent_color = getWhosTurn() == Piece.PieceColor.White ? Piece.PieceColor.Black : Piece.PieceColor.White;
        var dirs_info = get_eight_dirs_pieces(coord);

        foreach (var trans in new (int,int)[]{(1,1),(-1,1),(1,-1),(-1,-1)}){
            if (dirs_info.ContainsKey(Tuple.Create(trans.Item1, trans.Item2))){
                var (piece, neighbour_coord) = dirs_info[Tuple.Create(trans.Item1, trans.Item2)];
                if (piece.GetColor() == getWhosTurn()){
                    continue;
                }
                //Queen Diagonal and Bishop
                if (piece.GetPieceType() == Piece.PieceType.Bishop || piece.GetPieceType() == Piece.PieceType.Queen){
                    return true;
                }
                //King Diagonal
                if (piece.GetPieceType() == Piece.PieceType.King){
                    if (BigInteger.Abs(coord.Item1 - neighbour_coord.Item1) == 1 && 
                    BigInteger.Abs(coord.Item2 - neighbour_coord.Item2) == 1) {
                        return true;
                    }
                }
            }
        }

        foreach (var trans in new (int,int)[]{(1,0),(-1,0),(0,-1),(0,1)}){
            if (dirs_info.ContainsKey(Tuple.Create(trans.Item1, trans.Item2))){
                var (piece, neighbour_coord) = dirs_info[Tuple.Create(trans.Item1, trans.Item2)];
                if (piece.GetColor() == getWhosTurn()){
                    continue;
                }
                //Queen Vertical/Horizontal and Rook
                if (piece.GetPieceType() == Piece.PieceType.Rook || piece.GetPieceType() == Piece.PieceType.Queen){
                    return true;
                }
                //King Vertical/Horizontal
                if (piece.GetPieceType() == Piece.PieceType.King){
                    if (BigInteger.Abs(coord.Item1 - neighbour_coord.Item1) == 1 || 
                    BigInteger.Abs(coord.Item2 - neighbour_coord.Item2) == 1) {
                        return true;
                    }
                }
            }
        }
        //Knight
        foreach (var knight_trans in Move.KNIGHT_MOVES){
            var new_coord = Tuple.Create(coord.Item1 + knight_trans.Item1, coord.Item2 + knight_trans.Item2);
            if (current_board.ContainsKey(new_coord)){
                if (current_board[new_coord].GetPieceType() == Piece.PieceType.Knight && 
                    current_board[new_coord].GetColor() != getWhosTurn()){
                    return true;
                }
            }
        }

        //Pawn

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
    
    private bool AllowEnPassant(){
        if (moves_stack.Count == 0){
            return false;
        }

        var prev_move = moves_stack[moves_stack.Count-1];
        if (current_board[prev_move.GetToCoord()].GetPieceType() == Piece.PieceType.Pawn){
            if (Math.Abs((int) (prev_move.GetToCoord().Item2 - prev_move.GetFromCoord().Item2)) == 2){
                return true;
            }
        }

        return false;
    }

    //Return
    //Direction: Piece, Coord
    private Dictionary<Tuple<int,int>, Tuple<Piece, Tuple<BigInteger, BigInteger>>> get_eight_dirs_pieces(Tuple<BigInteger, BigInteger> square_coord){
        var temp_dist_dict = new Dictionary<Tuple<int,int>, Tuple<BigInteger, Piece, Tuple<BigInteger, BigInteger>>>();

        foreach (var cur_piece in current_board){
            if (cur_piece.Key == square_coord){
                continue;
            }

            var DisX = BigInteger.Abs(cur_piece.Key.Item1 - square_coord.Item1);
            var DisY = BigInteger.Abs(cur_piece.Key.Item2 - square_coord.Item2);

            if (DisX == DisY || DisX == 0 || DisY == 0){
                var x_change = (int)
                BigInteger.Max(
                    BigInteger.Min(
                        cur_piece.Key.Item1 - square_coord.Item1, 
                        new BigInteger(1))
                    , new BigInteger(-1));
                var y_change = (int)
                BigInteger.Max(
                    BigInteger.Min(
                        cur_piece.Key.Item2 - square_coord.Item2, 
                        new BigInteger(1))
                    , new BigInteger(-1));

                var dist = BigInteger.Max(DisX, DisY);
                if (temp_dist_dict.ContainsKey( Tuple.Create<int, int>(x_change, y_change) )){
                    if (dist < temp_dist_dict[Tuple.Create<int, int>(x_change, y_change)].Item1){
                        temp_dist_dict[Tuple.Create<int, int>(x_change, y_change)] = 
                            Tuple.Create(dist, cur_piece.Value, cur_piece.Key);
                    }
                } else {
                    temp_dist_dict[Tuple.Create<int, int>(x_change, y_change)] = 
                        Tuple.Create(dist, cur_piece.Value, cur_piece.Key);
                }
            }
        }

        var output = new Dictionary<Tuple<int,int>, Tuple<Piece, Tuple<BigInteger, BigInteger>>>();
        foreach (var pair in temp_dist_dict){
            output[pair.Key] = Tuple.Create(pair.Value.Item2, pair.Value.Item3);
        }

        return output;
    }

}
