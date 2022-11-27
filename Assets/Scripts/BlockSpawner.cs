using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private Sprite gargoyleArt, mansionArt, tenementArt, riverArt;
    private Dictionary<string, Sprite> TileArt;
    public int MaxBlockSize;

    private void Awake()
    {
        TileArt = new Dictionary<string, Sprite>();
        
        TileArt.Add("G", gargoyleArt);
        TileArt.Add("M", mansionArt);
        TileArt.Add("T", tenementArt);
        TileArt.Add("R", riverArt);
    }

    public void GenerateBlock()
    {
        GameObject NewBlock; 
        NewBlock = new GameObject("NewBlock");
        NewBlock.transform.parent = transform;
        NewBlock.transform.localPosition = Vector3.zero;
        NewBlock.AddComponent<Block>();
        //NewBlock.AddComponent<BoxCollider>();
        int blockSize = Random.Range(1, MaxBlockSize);
        NewBlock.GetComponent<Block>().GenerateTiles(NewBlock.transform, blockSize, TileArt);
    }
    
    void Start()
    {
        GenerateBlock();
    }

}
