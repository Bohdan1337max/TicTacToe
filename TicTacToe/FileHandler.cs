using System;
using System.IO;
using System.Linq;

namespace TicTacToe;

public class FileHandler
{
    private static string _pathX = @".\Models\X.txt";
    private static string _pathO = @".\Models\O.txt";
    public int CellWidth { get; set; }
    public int CellHeight { get; set; }


    public void FindCellSize()
    {
        var contentX = File.ReadLines(_pathX).ToArray();
        var maxLengthX = contentX.Max(s => s.Length);
        var linesCountX = contentX.Length;
        var contentO = File.ReadLines(_pathO).ToArray();
        var maxLengthO = contentO.Max(s => s.Length);
        var linesCountO = contentO.Length;
        CellWidth = Math.Max(maxLengthX, maxLengthO);
        CellHeight = Math.Max(linesCountX, linesCountO);
    }
    
    public string[] GetSignPicture(GameSigns currentSign)
    {
        var content = currentSign switch
        {
            GameSigns.X => File.ReadAllLines(_pathX).ToArray(),
            GameSigns.O => File.ReadAllLines(_pathO).ToArray(),
            _ => Array.Empty<string>()
        };

        return content;
    }
}