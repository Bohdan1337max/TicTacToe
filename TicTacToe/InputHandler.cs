using System;

namespace TicTacToe;

public class InputHandler
{
    private int _y;
    private int _x;

    public int X
    {
        get => _x;
        set => _x = Math.Clamp(value,0,2);
    }

    public int Y
    {
        get => _y;
        set => _y = Math.Clamp(value,0,2);
    }


    public void Handle(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.UpArrow:
                Y--;
                break;
            case ConsoleKey.DownArrow:
                Y++;
                break;
            case ConsoleKey.LeftArrow:
                X--;
                break;
            case ConsoleKey.RightArrow:
                X++;
                break;
            
        }
    }
    
    
    
}