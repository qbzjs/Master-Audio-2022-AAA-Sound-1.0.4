using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int size;

    [SerializeField] private LineRenderer gridLine;
    [SerializeField] private Transform bottomLeft, topRight, gridParent;
    private float gridUnit;
    
    private Grid grid;
    
    // Start is called before the first frame update
    void Start()
    {
        //TODO make this default Tile value stand for an "empty" tile
        grid = new Grid(size, new Tile());
        DrawGrid();
    }

    private void DrawGrid()
    {
        Vector2 tr = topRight.position;
        Vector2 bl = bottomLeft.position;
        Vector2 dist = topRight.position - bottomLeft.position;
        gridUnit = Mathf.Min(dist.x, dist.y) / size;
        float xLeft = bl.x;
        float xRight = bl.x + size * gridUnit;
        float yTop = bl.y + size * gridUnit;
        float yBottom = bl.y;
        
        for(int x = 0; x <= size; x++ )
        {
            float xPos = bl.x + gridUnit * x;
            DrawGridLine(xPos, yBottom, xPos, yTop);
        }
        
        for(int y = 0; y <= size; y++ ){
            float yPos = bl.y + gridUnit * y;
            DrawGridLine(xLeft, yPos, xRight, yPos);        
        }  
    }

    private void DrawGridLine(float x1, float y1, float x2, float y2)
    {
        Instantiate(gridLine, gridParent);
        Vector3[] positions = {new Vector3(x1, y1, 0), new Vector3(x2, y2, 0)};
        gridLine.SetPositions(positions);
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