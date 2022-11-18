using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Block : MonoBehaviour
{
    public List<GameObject> Tiles = new List<GameObject>();
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
    private Vector3 currPos;

    private Camera cam;
    private Vector3 dragOffset;
    [SerializeField] private float speed = 2;
    
    public void GenerateTiles(Transform parentTransform, int blockSize, GameObject[] tileOptions)
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
            GameObject myTile = Instantiate(tileOptions[rand], parentTransform);
            myTile.transform.localScale *= GridManager.Instance.GridUnit;
            myTile.transform.position = currPos;
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

    void OnMouseDown()
    {
        dragOffset = transform.position - GetMousePos();
    }
    
    void OnMouseDrag()
    {
        transform.position = GridManager.Instance.SnapToGrid( dragOffset + GetMousePos());
    }

    /*private void OnMouseUp()
    {
        throw new NotImplementedException();
    }*/

    Vector3 GetMousePos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        mousePos.z = 0;
        return mousePos;
    }
}
