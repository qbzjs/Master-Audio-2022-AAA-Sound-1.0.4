using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Directions
{
    /// <summary>
    /// Cardinal directions starting from right and going counterclockwise.
    /// right, up, left, down.
    /// </summary>
    public static Vector2Int[] Cardinal = {
        Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down
    };
    
    /// <summary>
    /// Ordinal (diagonal) directions starting from right/up and going counterclockwise.
    /// right/up, left/up, left/down, right/down.
    /// </summary>
    public static Vector2Int[] Ordinal = {
        Vector2Int.right + Vector2Int.up, 
        Vector2Int.left + Vector2Int.up, 
        Vector2Int.left + Vector2Int.down,
        Vector2Int.right + Vector2Int.down
    };
    
    /// <summary>
    /// Ordinal + Cardinal directions starting from right and going counterclockwise.
    /// right, right/up, up, left/up, left, left/down, down, right/down.
    /// </summary>
    public static Vector2Int[] Compass = {
        Vector2Int.right,
        Vector2Int.right + Vector2Int.up, 
        Vector2Int.up, 
        Vector2Int.left + Vector2Int.up, 
        Vector2Int.left, 
        Vector2Int.left + Vector2Int.down,
        Vector2Int.down,
        Vector2Int.right + Vector2Int.down
    };
}
