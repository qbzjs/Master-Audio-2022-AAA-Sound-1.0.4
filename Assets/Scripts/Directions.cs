using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dir{
    Up, Down, Left, Right
}

public class Direction
{
    private static float EPSILON = 0.001f;
    Direction(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    Direction(Vector2Int d)
    {
        x = d.x;
        y = d.y;
    }

    Direction(Vector2 d)
    {
        x = y = 0;
        if (d.x > EPSILON)
        {
            x = 1;
        } else if (d.x < -EPSILON)
        {
            x = -1;
        }
        
        if (d.y > EPSILON)
        {
            y = 1;
        } else if (d.y < -EPSILON)
        {
            y = -1;
        }

        if (x != 0 && y != 0)
        {
            if (Mathf.Abs(d.x) < Mathf.Abs(d.y))
            {
                x = 0;
            }
            else
            {
                y = 0;
            }
        }
    }

    Direction(Dir d)
    {
        if (d == Dir.Up)
        {
            x = 0;
            y = 1;
        } else if (d == Dir.Down)
        {
            x = 0;
            y = -1;
        } else if (d == Dir.Left)
        {
            x = -1;
            y = 0;
        } else if (d == Dir.Right)
        {
            x = 1;
            y = 0;
        }
    }
    private int x, y;
    public int X => x;
    public int Y => y;
}

public class Directions
{
    
}
