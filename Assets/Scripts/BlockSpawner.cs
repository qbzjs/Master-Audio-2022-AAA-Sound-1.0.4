using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class BlockSpawner : Singleton<BlockSpawner>
{
    private Dictionary<string, Sprite> TileArt;
    public int MaxBlockSize;

    private void Awake()
    {
        TileArt = new Dictionary<string, Sprite>();
        
        TileArt.Add("GA", ArtManager.Instance.gargoyleArt);
        TileArt.Add("MA", ArtManager.Instance.mansionArt);
        TileArt.Add("TE", ArtManager.Instance.tenementArt);
        TileArt.Add("RI", ArtManager.Instance.riverArt);
        TileArt.Add("CH", ArtManager.Instance.churchArt);
        TileArt.Add("WI", ArtManager.Instance.churchWingArt);
        TileArt.Add("GR", ArtManager.Instance.graveyardArt);
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
