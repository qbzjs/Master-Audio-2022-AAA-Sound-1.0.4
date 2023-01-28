using System;
public class PlacedEvent : DefaultEvent
{
    public int xPos { get; set; }
    public int yPos { get; set; }
    public PlacedEvent(int x, int y) : base(x, y)
    {
    }
}

