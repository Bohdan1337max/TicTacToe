using System;

namespace TicTacToe;

public struct Pixel
{
    public Pixel()
    {
        Color = ConsoleColor.Black;
        Char = ' ';
    }

    public ConsoleColor Color { get; set; }
    public char Char { get; set; } = ' ';
    
}