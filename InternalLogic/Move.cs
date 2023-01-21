using Godot;
using System;
using System.Collections.Generic;

public class Move {
    private bool disrupted_left_cast = false;
    private bool disrupted_right_cast = false;
    private bool is_promotion = false;
    private bool is_en_passant = false;

    public static readonly Tuple<int,int>[] KNIGHT_MOVES = new Tuple<int,int>[]{
        Tuple.Create<int,int>(1,2),
        Tuple.Create<int,int>(2,1),
        Tuple.Create<int,int>(-1,2),
        Tuple.Create<int,int>(2,-1),
        Tuple.Create<int,int>(1,-2),
        Tuple.Create<int,int>(-2,1),
        Tuple.Create<int,int>(-1,-2),
        Tuple.Create<int,int>(-2,-1),
    };

    public static readonly Tuple<int,int>[] BISHOP_MOVES = new Tuple<int,int>[]{
        Tuple.Create<int,int>(1,1),
        Tuple.Create<int,int>(-1,-1),
        Tuple.Create<int,int>(1,-1),
        Tuple.Create<int,int>(-1,1),
    };

    public static readonly Tuple<int,int>[] ROOK_MOVES = new Tuple<int,int>[]{
        Tuple.Create<int,int>(1,0),
        Tuple.Create<int,int>(-1,0),
        Tuple.Create<int,int>(0,-1),
        Tuple.Create<int,int>(0,1),
    };

    //Hard Coded for Castling Check
    public static readonly Tuple<int,int>[] WHITE_KING_SIDE_CASTLE_SQUARES = new Tuple<int,int>[]{
        Tuple.Create<int,int>(1,0),
        Tuple.Create<int,int>(2,0),
    };

    public static readonly Tuple<int,int>[] WHITE_QUEEN_SIDE_CASTLE_SQUARES = new Tuple<int,int>[]{
        Tuple.Create<int,int>(4,0),
        Tuple.Create<int,int>(5,0),
    };

    public static readonly Tuple<int,int>[] BLACK_KING_SIDE_CASTLE_SQUARES = new Tuple<int,int>[]{
        Tuple.Create<int,int>(1,7),
        Tuple.Create<int,int>(2,7),
    };

    public static readonly Tuple<int,int>[] BLACK_QUEEN_SIDE_CASTLE_SQUARES = new Tuple<int,int>[]{
        Tuple.Create<int,int>(4,7),
        Tuple.Create<int,int>(5,7),
    };
}