using System;
using System.Collections;
using System.Collections.Generic;
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

    private Camera cam;
    
    public void GenerateTiles(Transform parentTransform, int blockSize, Sprite[] tileOptions)
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
            int rand = Random.Range(0, tileOptions.Length);
            ITile myTile = new Gargoyle(tileOptions, transform, currPos);
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

    public void OnMouseDown()
    {
        dragOffset = transform.position - GetMousePos();
    }
    
    public void OnMouseDrag()
    {
        transform.position = GridManager.Instance.SnapToGrid( dragOffset + GetMousePos());
    }

    public void OnMouseUp()
    {
        GridManager.Instance.PlaceBlock(this);
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
            tile.TileObject.GetComponent<DragChild>().enabled = false;
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
