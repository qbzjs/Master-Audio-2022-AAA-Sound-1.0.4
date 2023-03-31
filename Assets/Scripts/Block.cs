using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DarkTonic.MasterAudio;
using Unity.Mathematics;
using Unity.VisualScripting;
using Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class Block : MonoBehaviour, IDragParent
{
    public List<ITile> Tiles = new List<ITile>();
    
    private List<Vector2Int> optionsList = new List<Vector2Int>();
    private HashSet<Vector2Int> options = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> taken = new HashSet<Vector2Int>();
    private Vector2Int currPos;
    public Vector3 dragOffset;
    
    public bool held = false;
    public bool isMaw = false;
    public bool clicked = false;

    private Camera cam;

    Vector2Int prevPosition = new Vector2Int(0,0);
    Vector2Int newPosition = new Vector2Int(0,0);
    
    public void GenerateFakeTiles(Transform parentTransform, int blockSize, List<string> names, List<Vector2Int> positions)
    {
        Vector3 position = parentTransform.position;
        for(int i = 0; i < blockSize; i++)
        {
            string tileID = names[i];
            ITile myTile = TileFactory.CreateTile(Type.GetType(tileID), transform, Vector2IntToWorldPosition(positions[i], position));
           
            myTile.TileObject.transform.localScale *= GridManager.Instance.GridUnit;
            myTile.TileObject.AddComponent<DragChild>().parent = this;
            myTile.TileObject.AddComponent<MouseOverTile>().Tile = myTile;

            Tiles.Add(myTile);
        }

    }

    public void GenerateTiles(Transform parentTransform, int blockSize)
    {
        Vector3 position = parentTransform.position;
        options.Add(currPos);
        optionsList.Add(currPos);
        for(int i = 0; i < blockSize; i++){
            
            string tileID = DeckManager.Instance.Draw();
            
            //tileID needs to be the exact name of the class to instantiate. (Ezra) this function is a little gross (sorry)
            //  it was the best way I could find to decouple the subclasses from this file. Open to ideas on 
            //  how to fix this!
            ITile myTile = TileFactory.CreateTile(Type.GetType(tileID), transform, Vector2IntToWorldPosition(currPos, position));

            myTile.TileObject.transform.localScale *= GridManager.Instance.GridUnit;
            myTile.TileObject.AddComponent<DragChild>().parent = this;
            myTile.TileObject.AddComponent<MouseOverTile>().Tile = myTile;

            Tiles.Add(myTile);
            options.Remove(currPos);
            optionsList.Remove(currPos);
            taken.Add(currPos);
            for(int j = 0; j < 2; j++) //Note, this only goes right and up. This 
                                       // is a horrible hack to make it not go off the left
                                       // side of the screen. Yay! Programming!
            {
                Vector2Int tmpPos = currPos + Directions.Cardinal[j];
                if(!taken.Contains(tmpPos) && !options.Contains(tmpPos)){
                    options.Add(tmpPos);
                    optionsList.Add(tmpPos);
                }
            }
            int tmpIdx = Random.Range(0, optionsList.Count);
            currPos = optionsList[tmpIdx];
        }
    }
    
    public void GenerateMaw(Transform parentTransform)
    {
        isMaw = true;
        Vector3 position = parentTransform.position;
        
        ITile myTile = TileFactory.CreateTile(Type.GetType("Maw"), transform, position);
        myTile.TileObject.transform.localScale *= GridManager.Instance.GridUnit;
        myTile.TileObject.AddComponent<DragChild>().parent = this;
        myTile.TileObject.AddComponent<MouseOverTile>().Tile = myTile;
        Tiles.Add(myTile);
    }

    public Vector3 Vector2IntToWorldPosition(Vector2Int offset, Vector3 origin)
    {
        float unit = GridManager.Instance.GridUnit;
        return new Vector3(offset.x * unit, offset.y * unit, 0) + origin;
    }

    void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (clicked == true)
        {
            FollowMousePos();
            if (Input.GetMouseButtonDown(1))
            {
                Rotate(-90);
            }
            if(Input.GetMouseButtonDown(0) && GameManager.Instance.dragging)
            {
                OnMouseDown();
                OnMouseUp();
            }
            else{
                GameManager.Instance.dragging = true;
            }
        }
        
    }

    /// <summary>
    /// Rotates the tiles composing this block, around the mouse if dragging or around
    /// its center if not dragging
    /// </summary>
    /// <param name="degrees">How many degrees counterclockwise you want to rotate</param>
    private void Rotate(float degrees)
    {
        float rad = degrees * Mathf.PI / 180;
        Matrix4x4 rotMat = Matrix4x4.Rotate(quaternion.RotateZ(rad));
        Vector3 centerOfGravity = Vector3.zero;
        Vector3 lastTile = Vector3.zero;
        foreach (ITile tile in Tiles)
        {
            lastTile = tile.TileObject.transform.position;
            Vector3 localSpacePosition = tile.TileObject.transform.localPosition - centerOfGravity;
            tile.TileObject.transform.localPosition = centerOfGravity + (Vector3)(rotMat *  localSpacePosition);
        }
    }

    public void OnMouseDown()
    {
        dragOffset = transform.position - GetMousePos();
        dragOffset.z = 0;
        if(clicked == true){
            clicked = false;
        }else{
            clicked = true;
        }
    }
    
    public void FollowMousePos()
    {
        bool onGrid = true;

        Vector3 mousePos = GetMousePos();
        this.gameObject.transform.position = mousePos;
            
        foreach (ITile tile in Tiles)
        {
            onGrid &= GridManager.Instance.OverGrid(tile.Position());
        }

        if (onGrid)
        {
            newPosition = GridManager.Instance.WorldToGridPos(transform.position);
            if (prevPosition != newPosition)
            {
                MasterAudio.PlaySound("Click");
            } 
            transform.position = GridManager.Instance.GridToWorldPos(newPosition);
        } else
        {
            transform.position = dragOffset + GetMousePos();
        }
        
        foreach(ITile tile in Tiles)
        {
            tile.TileObject.GetComponent<SpriteRenderer>().color = PlaceIndicatorColor(onGrid);
            tile.TileObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        
        prevPosition = GridManager.Instance.WorldToGridPos(transform.position);
    }

    private Color PlaceIndicatorColor(bool onBoard)
    {
        
        Color offBoard = Color.white;
        Color valid = new Color(0.71f, 1f, 0.72f);
        Color invalid = new Color(1f, 0.56f, 0.58f);

        if (!onBoard)
        {
            return offBoard;
        }

        if (isMaw)
        {
            return valid;
        }
        
        bool allValid = true;
        foreach (ITile tile in Tiles)
        {
            Vector2Int location = GridManager.Instance.WorldToGridPos(tile.Position());
            if (GridManager.Instance.GetTile(location.x, location.y).GetType().ToString() != "Wasteland")
            {
                return invalid;
            }
        }

        return valid;
    }

    public void OnMouseUp()
    {
        GridManager.Instance.PlaceBlock(this);
        if (!HoldingCell.Instance.holding && HoldingCell.Instance.over && !isMaw)
        {
            held = true;
            HoldingCell.Instance.holding = true;
            clicked = false;
            BlockSpawner.Instance.GenerateBlock();
        }

        foreach(ITile tile in Tiles)
        {
            tile.TileObject.GetComponent<SpriteRenderer>().color = Color.white;
        } 
        if (clicked == false){
            GameManager.Instance.dragging = false;
        }
    }

    public Vector3 GetMousePos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        mousePos.z = 0;
        return mousePos;
    }

    public void Destroy()
    {
        foreach (var tile in Tiles)
        {
            tile.TileObject.GetComponent<SpriteRenderer>().sortingOrder = -200;
            Destroy(tile.TileObject.GetComponent<DragChild>());
        }
        Destroy(this);
    }
}

public interface IDragParent
{
    public void OnMouseUp();
    public void OnMouseDown();
}