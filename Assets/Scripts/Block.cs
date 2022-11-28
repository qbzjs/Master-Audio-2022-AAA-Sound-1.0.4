using System;
using System.Collections;
using System.Collections.Generic;
using Scripts;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Block : MonoBehaviour, IDragParent
{
    public List<ITile> Tiles = new List<ITile>();
    
    private List<Vector3> Directions = new List<Vector3>()
    {
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, -1, 0),
    };
    private List<Vector3> optionsList = new List<Vector3>();
    private HashSet<Vector3> options = new HashSet<Vector3>();
    private HashSet<Vector3> taken = new HashSet<Vector3>();
    private Vector3 currPos, dragOffset;
    private bool dragging;

    private Camera cam;
    
    public void GenerateTiles(Transform parentTransform, int blockSize, Dictionary<string, Sprite> tileOptions)
    {
        Vector3 position = parentTransform.position;

        Directions[0] = Directions[0] * GridManager.Instance.GridUnit;
        Directions[1] = Directions[1] * GridManager.Instance.GridUnit;
        Directions[2] = Directions[2] * GridManager.Instance.GridUnit;
        Directions[3] = Directions[3] * GridManager.Instance.GridUnit;
        currPos = position;
        options.Add(currPos);
        optionsList.Add(currPos);
        Vector3 tmpPos;
        int tmpIdx;
        for(int i = 0; i < blockSize; i++){
            int rand = Random.Range(0, 7); //TODO un-magic-number this

            ITile myTile;

            switch (rand)
            {
                case 0:
                    myTile = new Gargoyle(tileOptions["GA"], transform, currPos);
                    break;
                case 1:
                    myTile = new Mansion(tileOptions["MA"], transform, currPos);
                    break;
                case 2:
                    myTile = new Tenement(tileOptions["TE"], transform, currPos);
                    break;
                case 3:
                    myTile = new River(tileOptions["RI"], transform, currPos);
                    break;
                case 4:
                    myTile = new Church(tileOptions["CH"], transform, currPos);
                    break;
                case 5:
                    myTile = new Wing(tileOptions["WI"], transform, currPos);
                    break;
                case 6:
                    myTile = new Graveyard(tileOptions["GR"], transform, currPos);
                    break;
                default:
                    myTile = new Gargoyle(tileOptions["GA"], transform, currPos);
                    break;
            }
            
            
            myTile.TileObject.transform.localScale *= GridManager.Instance.GridUnit;
            myTile.TileObject.AddComponent<BoxCollider>();
            myTile.TileObject.AddComponent<DragChild>().parent = this;
            Tiles.Add(myTile);
            options.Remove(currPos);
            optionsList.Remove(currPos);
            taken.Add(currPos);
            for(int j = 0; j < 4; j++){
                tmpPos = currPos + Directions[j];
                if(!taken.Contains(tmpPos) && !options.Contains(tmpPos)){
                    options.Add(tmpPos);
                    optionsList.Add(tmpPos);
                }
            }
            tmpIdx = Random.Range(0, optionsList.Count);
            currPos = optionsList[tmpIdx]; 
        }
        
    }

    void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(90);
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(-90);
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
        /*foreach (ITile tile in Tiles)
        {
            centerOfGravity += tile.TileObject.transform.localPosition;
        }
        centerOfGravity /= Tiles.Count;*/
        foreach (ITile tile in Tiles)
        {
            Vector3 localSpacePosition = tile.TileObject.transform.localPosition - centerOfGravity;
            tile.TileObject.transform.localPosition = centerOfGravity + (Vector3)(rotMat *  localSpacePosition);
        }
    }

    public void OnMouseDown()
    {
        dragOffset = transform.position - GetMousePos();
        dragOffset.z = 0;
        dragging = true;
    }
    
    public void OnMouseDrag()
    {
        bool onGrid = true;
        foreach (ITile tile in Tiles)
        {
            onGrid &= GridManager.Instance.OverGrid(tile.Position());
        }

        if (onGrid)
        {
            transform.position = GridManager.Instance.SnapToGrid( dragOffset + GetMousePos());
        }
        else
        {
            transform.position = dragOffset + GetMousePos();
        }
    }

    public void OnMouseUp()
    {
        GridManager.Instance.PlaceBlock(this);
        dragging = false;
    }

    Vector3 GetMousePos()
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
    public void OnMouseDrag();
}
