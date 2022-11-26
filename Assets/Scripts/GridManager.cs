using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : Singleton<GridManager>
{
    [SerializeField]
    private int size;

    [SerializeField] private LineRenderer gridLine;
    [SerializeField] private Transform bottomLeft, topRight, gridParent;
    private float gridUnit;

    public float GridUnit {
        get => gridUnit;
    }
    
    private Grid grid;
    
    // Start is called before the first frame update
    void Start()
    {
        //TODO make this default Tile value stand for an "empty" tile
        grid = new Grid(size, new EmptyTile());
        DrawGrid();
    }

    //Uses copies of the gridLine prefab to draw a grid.
    private void DrawGrid()
    {
        gridLine.enabled = true;
        
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

        gridLine.enabled = false;
    }

    private void DrawGridLine(float x1, float y1, float x2, float y2)
    {
        LineRenderer newLine = Instantiate(gridLine, gridParent);
        Vector3[] positions = {new Vector3(x1, y1, 0), new Vector3(x2, y2, 0)};
        newLine.SetPositions(positions);
    }

    public void PlaceBlock(Block block)
    {
        foreach (ITile tile in block.Tiles)
        {
            Vector2Int gridPos = WorldToGridPos(tile.TileObject.transform.position);
            if (!grid.InRange(gridPos))
            {
                Debug.Log("Out of bounds!"); //TODO replace with user feedback
                return;
            }
            if (!grid[gridPos.x, gridPos.y].Destructible())
            {
                Debug.Log("You can't place a block here!");
                return;
            }
        }
        foreach (ITile tile in block.Tiles)
        {
            Vector2Int gridPos = WorldToGridPos(tile.TileObject.transform.position);
            DestroyTile(gridPos);
            grid[gridPos.x, gridPos.y] = tile;
        }

        block.Destroy();
        GameManager.Instance.PlacedBlock();
    }

    private void DestroyTile(Vector2Int pos)
    {
        Destroy(grid[pos.x, pos.y].TileObject);
        //TODO add more logic here about destroying old tiles;
    }

    /// <summary>
    /// Rounds a Vector3 to the nearest position on the grid, does nothing if not on the grid. For snapping tiles to the grid.
    /// </summary>
    public Vector3 SnapToGrid(Vector3 worldPos)
    {
        Vector2Int gridPos = WorldToGridPos(worldPos);
        return (grid.InRange(gridPos.x, gridPos.y) ? GridToWorldPos(gridPos) : worldPos);
    }
    
    /// <summary>
    /// translates the world position to grid position
    /// </summary>
    /// <param name="worldPos">world position to be converted</param>
    /// <returns>the grid position equivalent: note that the value may be out of range</returns>
    public Vector2Int WorldToGridPos(Vector3 worldPos)
    {
        Vector3 diff = worldPos - bottomLeft.position;
        Vector3 scaled = diff / gridUnit;
        return new Vector2Int(Mathf.FloorToInt(scaled.x), Mathf.FloorToInt(scaled.y));
    }

    /// <summary>
    /// Returns true if the position is over the grid, false otherwise
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool OverGrid(Vector3 position)
    {
        return grid.InRange(WorldToGridPos(position));
    }

    public Vector3 GridToWorldPos(Vector2Int gridPos)
    {
        float x = bottomLeft.position.x + (gridPos.x + 0.5f) * gridUnit;
        float y = bottomLeft.position.y + (gridPos.y + 0.5f) * gridUnit;
        return  new Vector3(x, y,0);
    }

    public int UpdateScore()
    {
        int score = 0;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                score += grid[x, y].CalculateScore();
            }
        }
        return score;
    }

    public ITile GetTile(int x, int y)
    {
        return grid[x, y];
    }
}

public class Grid
{
    private ITile[,] grid;
    private int size;
    
    public Grid(int _size, ITile defaultValue)
    {
        size = _size;
        grid = new ITile[size,size];
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                grid[x, y] = defaultValue;
            }
        }
    }

    public bool InRange(Vector2Int pos)
    {
        return InRange(pos.x, pos.y);
    }
    
    public bool InRange(int x, int y)
    {
        return !(x < 0 || y < 0 || x >= size || y >= size);
    }
    
    //This is the syntax for array properties apparently! 
    public ITile this [int x, int y]
    {
        get //To call this the syntax is 
            // Grid[x, y];
        {
            if (InRange(x, y))
            {
                return grid[x, y];
            }
            else
            {
                return new EmptyTile();
            }
        }
        set //To call this the syntax is 
            // Grid[x, y] = newValue;
        {
            grid[x, y] = value;
        }
    }
}