using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField]
        private int size;

        public int Size
        {
            get => size;
        }
        
        private int nextSize = 0;

        [SerializeField] private LineRenderer gridLine;
        [SerializeField] private Transform bottomLeft, topRight, gridParent;
        private float gridUnit;
        private List<GameObject> gridLines;

        public float GridUnit
        {
            get => gridUnit;
        }

        private Grid grid;

        public Grid Grid => grid;

        public void Initialize()
        {
            if (nextSize == 0)
            {
                nextSize = size;
            }
            else
            {
                size = nextSize;
            }
            if (grid != null)
            {
                ClearGrid();
            }
            grid = new Grid(size, new Wasteland());
            DrawGrid();
            PlaceFountain();
        }


        public int GetSize()
        {
            return size;
        }

        public void ChangeSize(int delta)
        {
            nextSize += delta;
        }

        //Uses copies of the gridLine prefab to draw a grid.
        private void DrawGrid()
        {
            if (gridLines == null)
            {
                gridLines = new List<GameObject>();
            }
            else
            {
                foreach (GameObject line in gridLines)
                {
                    Destroy(line);
                }

                gridLines.Clear();
            }
            
            gridLine.enabled = true;

            Vector2 tr = topRight.position;
            Vector2 bl = bottomLeft.position;
            Vector2 dist = topRight.position - bottomLeft.position;
            gridUnit = Mathf.Min(dist.x, dist.y) / size;
            float xLeft = bl.x;
            float xRight = bl.x + size * gridUnit;
            float yTop = bl.y + size * gridUnit;
            float yBottom = bl.y;

            for (int x = 0; x <= size; x++)
            {
                float xPos = bl.x + gridUnit * x;
                DrawGridLine(xPos, yBottom, xPos, yTop);
            }

            for (int y = 0; y <= size; y++)
            {
                float yPos = bl.y + gridUnit * y;
                DrawGridLine(xLeft, yPos, xRight, yPos);
            }

            gridLine.enabled = false;
        }

        private void DrawGridLine(float x1, float y1, float x2, float y2)
        {
            LineRenderer newLine = Instantiate(gridLine, gridParent);
            Vector3[] positions = { new Vector3(x1, y1, 0), new Vector3(x2, y2, 0) };
            newLine.SetPositions(positions);
            gridLines.Add(newLine.gameObject);
        }

        public void PlaceBlock(Block block)
        {
            TweenManager.Instance.PlaceBlock(block.gameObject, delegate
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
                    PlaceTile(tile, WorldToGridPos(tile.TileObject.transform.position));
                    tile.WhenPlaced();
                }

                foreach (ITile tile in block.Tiles)
                {
                    ObserverManager.Instance.AddObserver(tile);
                }

                if (block.held)
                {
                    HoldingCell.Instance.holding = false;
                }
                else
                {
                    BlockSpawner.Instance.GenerateBlock();
                }
                block.Destroy();
                GameManager.Instance.PlacedBlock();
            });
        }

        private void PlaceFountain()
        {
            Vector2Int gridPos = grid.RandomPosition();
            PlaceTile("Fountain", gridPos);
        }

        public void PlaceTile(string tileClassName, Vector2Int gridPos)
        {
            ITile myTile = TileFactory.CreateTile(System.Type.GetType(tileClassName), transform, Vector3.zero);
            PlaceTile(myTile, gridPos);
        }
        
        private void PlaceTile(ITile tile, Vector2Int gridPos)
        {
            DestroyTile(gridPos);
            tile.xPos = gridPos.x;
            tile.yPos = gridPos.y;
            tile.TileObject.transform.position = GridToWorldPos(gridPos);
            grid[gridPos.x, gridPos.y] = tile;
        }

        private void DestroyTile(Vector2Int pos)
        {
            Wasteland tile = (Wasteland) grid[pos.x, pos.y];
            if (tile.TileObject == null)
                return;
            
            //Notifies all tiles that a tile has been destroyed, if they want to do something with that
            ForEach((int x, int y, ITile tile) =>
            {
                tile.WhenAnyDestroyed(pos.x, pos.y, grid[pos.x, pos.y]);
            });
            
            Destroy(grid[pos.x, pos.y].TileObject);

            ObserverManager.Instance.RemoveObserver(tile);

            ObserverManager.Instance.ProcessEvent(new GraveyardEvent(pos.x, pos.y));
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
            return new Vector3(x, y, 0);
        }

        public int UpdateScore()
        {
            int score = 0;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    score += grid[x, y].CalculateScore().score;
                }
            }
            return score;
        }

        public ITile GetTile(int x, int y)
        {
            return grid[x, y];
        }

        public void ClearGrid()
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    DestroyTile(new Vector2Int(x, y));
                }
            }
        }
        
        /// <summary>
        /// For convenience to loop through all the tiles on the board
        /// </summary>
        /// <param name="toApply">takes int x, int y and ITile tile. This is called for every position on the board</param>
        public static void ForEach(Action<int, int, ITile> toApply)
        {
            for (int x = 0; x < Instance.Size; x++)
            {
                for (int y = 0; y < Instance.Size; y++)
                {
                    toApply.Invoke(x, y, Instance.GetTile(x, y));
                }
            }
        }
    }

    public class Grid
    {
        private ITile[,] grid;
        private int size;

        public Grid(int _size, ITile defaultValue)
        {
            size = _size;
            grid = new ITile[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    grid[x, y] = defaultValue;
                }
            }
        }

        public Vector2Int RandomPosition()
        {
            return new Vector2Int(Random.Range(0, size), Random.Range(0, size));
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
        public ITile this[int x, int y]
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
                    return new Wasteland();
                }
            }
            set //To call this the syntax is 
                // Grid[x, y] = newValue;
            {
                grid[x, y] = value;
            }
        }
    }
}
