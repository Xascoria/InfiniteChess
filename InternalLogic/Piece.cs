using Godot;
using System;
using System.Collections.Generic;

public class Piece {
    public enum PieceType {
        King,
        Queen,
        Rook,
        Knight,
        Bishop,
        Pawn,
    }
    public enum PieceColor {
        White,
        Black
    }

    private PieceType type = PieceType.Pawn;
    private String name = "A Pawn";
    private PieceColor color = PieceColor.White;
    private int index = 0;

    public Piece(PieceType type, String name, PieceColor color, int index){
        this.type = type;
        this.name = name;
        this.color = color;
        this.index = index;
    }

    public PieceColor GetColor(){
        return this.color;
    }

    public PieceType GetPieceType(){
        return this.type;
    }

    public int GetIndex(){
        return this.index;
    }

    public String GetName(){
        return this.name;
    }
}