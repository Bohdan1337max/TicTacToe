public struct Pixel
{
    public Pixel()
    {
        _color = ConsoleColor.Black;
        _char = ' ';
    }

    public ConsoleColor _color { get; set; }
    public Char _char { get; set; } = ' ';
    
}