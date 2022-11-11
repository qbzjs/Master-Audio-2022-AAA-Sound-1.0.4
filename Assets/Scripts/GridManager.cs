using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int size;

    private Grid grid;
    
    // Start is called before the first frame update
    void Start()
    {
        //TODO make this default Tile value stand for an "empty" tile
        grid = new Grid(size, new Tile());
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class Grid
{
    private Tile[,] grid;
    private int size;
    
    public Grid(int _size, Tile defaultValue)
    {
        size = _size;
        grid = new Tile[size,size];
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                grid[x, y] = defaultValue;
            }
        }
    }

    public bool InRange(int x, int y)
    {
        return x < 0 || y < 0 || x >= size || y >= size;
    }
    
    //This is the syntax for array properties apparently! 
    public Tile this [int x, int y]
    {
        get //To call this the syntax is 
            // Grid[x, y];
        {
            if (InRange(x, y))
            {
                return new Tile(); //TODO should return some "out of range" value
            }
            else
            {
                return grid[x, y];
            }
        }
        set //To call this the syntax is 
            // Grid[x, y] = newValue;
        {
            grid[x, y] = value;
        }
    }
}