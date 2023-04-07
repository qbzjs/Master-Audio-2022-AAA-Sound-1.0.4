using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField]
        private int size;
        
        [SerializeField] private Transform ScorePopup;

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
            gridLine.gameObject.layer = LayerMask.NameToLayer("Show On End");
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
            GameManager.Instance.dragging = false;
            TweenManager.Instance.PlaceBlock(block.gameObject, delegate
            {
                foreach (ITile tile in block.Tiles)
                {
                    Vector2Int gridPos = WorldToGridPos(tile.TileObject.transform.position);
                    if (!grid.InRange(gridPos))
                    {
                        if(HoldingCell.Instance.over && (!HoldingCell.Instance.holding || block.held))
                        {   
                            LeanTween.cancel(block.gameObject);
                            LeanTween.cancel(Camera.main.gameObject);
                            return;
                        } 
                        if (!HoldingCell.Instance.over && block.held)
                        {
                            Vector3 finalPos = HoldingCell.Instance.transform.position;
                            LeanTween.cancel(block.gameObject);
                            LeanTween.cancel(Camera.main.gameObject);
                            LeanTween.move(block.gameObject, finalPos, 0.3f).setEaseInSine();
                            return;
                        }
                        else
                        {
                            Vector3 finalPos = block.isMaw ? BlockSpawner.Instance.mawSpawner.position : 
                                BlockSpawner.Instance.transform.position  ;
                            LeanTween.cancel(block.gameObject);
                            LeanTween.cancel(Camera.main.gameObject);
                            LeanTween.move(block.gameObject, finalPos, 0.3f).setEaseInSine();
                            return;
                        } 
                    }
                    if (grid[gridPos.x, gridPos.y].GetType().Name != "Wasteland" && !block.isMaw)
                    {
                        Vector3 finalPos = block.isMaw ? BlockSpawner.Instance.mawSpawner.position : 
                                                         BlockSpawner.Instance.transform.position  ;
                        if (block.held){
                            finalPos = HoldingCell.Instance.transform.position;
                        }
                        LeanTween.cancel(block.gameObject);
                        LeanTween.cancel(Camera.main.gameObject);
                        LeanTween.move(block.gameObject, finalPos, 0.3f).setEaseInSine();
                        return;
                    }
                } 
                foreach (ITile tile in block.Tiles)
                {
                    if (block.isMaw)
                    {
                        MasterAudio.PlaySound("BITE");
                        TweenManager.Instance.Callout("Maw Devours!", tile.Position());
                        GameManager.Instance.DestroyTile(WorldToGridPos(tile.TileObject.transform.position));
                    }
                    PlaceTile(tile, WorldToGridPos(tile.TileObject.transform.position));
                    if(!block.isMaw) 
                    {
                        DeckManager.Instance.Place(tile.GetType().FullName);
                    }
                }
                
                foreach (ITile tile in block.Tiles)
                {
                    tile.WhenPlaced();
                }

                if (block.held)
                {
                    HoldingCell.Instance.holding = false;
                }
                else
                {
                    if (block.isMaw)
                    {
                        BlockSpawner.Instance.GenerateMaw();
                    }
                    else
                    {
                        BlockSpawner.Instance.GenerateBlock();
                    }
                }
                if (!block.isMaw)
                {
                    TweenManager.Instance.PlaceBlockCards(block);
                }
                block.Destroy();
                GameManager.Instance.PlacedBlock();
                MasterAudio.PlaySound("Placed");
            });
        }

        private void PlaceFountain()
        {
            Vector2Int gridPos = grid.RandomPosition();
            PlaceTile("Fountain", gridPos);
        }

        //returns a copy of the TileObject
        public GameObject PlaceTile(string tileClassName, Vector2Int gridPos)
        {
            ITile myTile = TileFactory.CreateTile(Type.GetType(tileClassName), transform, Vector3.zero);
            myTile.TileObject.AddComponent<MouseOverTile>().Tile = myTile;
            return PlaceTile(myTile, gridPos);
        }
        
        private GameObject PlaceTile(ITile tile, Vector2Int gridPos)
        {
            DestroyTile(gridPos);
            tile.xPos = gridPos.x;
            tile.yPos = gridPos.y;
            tile.TileObject.layer = LayerMask.NameToLayer("Show On End");
            tile.TileObject.transform.position = GridToWorldPos(gridPos);
            if (grid.InRange(gridPos.x, gridPos.y)){
                tile.TileScore = grid[gridPos.x, gridPos.y].TileScore;
                grid[gridPos.x, gridPos.y] = tile;
            }else{
                 tile.TileScore = new Score(0);
            }

            return tile.TileObject;
        }

        //Destroys the data version of a tile at location pos, returns the TileObject,
        //so the caller can deal with the visuals
        public GameObject DestroyTile(Vector2Int pos, bool triggerEffects = true)
        {
            Wasteland tile = (Wasteland) grid[pos.x, pos.y];
            Action toInvoke = null;
            if (tile.TileObject == null)
                return null;

            if (triggerEffects)
            {
                //Notifies all tiles that a tile has been destroyed, if they want to do something with that
                ForEach((int x, int y, ITile tile) =>
                {
                    tile.WhenAnyDestroyed(pos.x, pos.y, grid[pos.x, pos.y]);
                });
                if (tile is IEffectOnDestroyed effectTile)
                {
                    toInvoke = effectTile.GetInvokeAfterDestroyed();
                }
            }

            GameObject toReturn = grid[pos.x, pos.y].TileObject;
            grid[pos.x, pos.y] = new Wasteland();
            grid[pos.x, pos.y].TileScore = tile.TileScore;
            toInvoke?.Invoke();
            return toReturn;
        }

        public void KillTile(Vector2Int pos)
        {
            Wasteland tile = (Wasteland)grid[pos.x, pos.y];
            if (tile.TileObject == null)
                return;

            //Notifies all tiles that a tile has been destroyed, if they want to do something with that
            ForEach((int x, int y, ITile tile) =>
            {
                tile.WhenAnyDestroyed(pos.x, pos.y, grid[pos.x, pos.y]);
            });

            Destroy(grid[pos.x, pos.y].TileObject);

            grid[pos.x, pos.y] = new Wasteland();
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
        
        /// <summary>
        /// Returns true if the point is in the grid, false otherwise
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool OverGrid(Vector2Int position)
        {
            return grid.InRange(position);
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
                    int oldScore = grid[x, y].TileScore.score;
                    int val = grid[x, y].CalculateScore().score;
                    int popupScore = val - oldScore;
                    string scorePopup = "";
                    if (popupScore != 0){
                        if (popupScore > 0) 
                        {
                            scorePopup  = "+" + popupScore.ToString();
                        }else 
                        {
                            scorePopup  = popupScore.ToString();
                        }
                        TweenManager.Instance.Callout(scorePopup, new Vector2Int(x,y));
                    }
                    score += val;
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
            ForEach((x, y, tile) => {EraseTile(tile);});
        }
        
        public void EraseTile(ITile tile)
        {
            Destroy(DestroyTile(new Vector2Int(tile.xPos, tile.yPos), false));
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
        
        /// <summary>
        /// For convenience to loop through all the tiles of a certain type
        /// </summary>
        /// <param name="toApply">takes int x, int y and T (ITile) tile. This is called for every position on the board</param>
        public static void ForEach<T>(Action<int, int, T> toApply)
        {
            for (int x = 0; x < Instance.Size; x++)
            {
                for (int y = 0; y < Instance.Size; y++)
                {
                    if (Instance.GetTile(x, y) is T t)
                    {
                        toApply.Invoke(x, y, t);   
                    }
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
