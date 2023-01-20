using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class BlockSpawner : Singleton<BlockSpawner>
{
    private Dictionary<string, Sprite> TileArt;
    [MinMaxSlider(1, 10)]
    public Vector2 minMaxSize;

    public void GenerateBlock()
    {
        GameObject NewBlock; 
        NewBlock = new GameObject("NewBlock");
        NewBlock.transform.parent = transform;
        NewBlock.transform.localPosition = Vector3.zero;
        NewBlock.AddComponent<Block>();
        int blockSize = Random.Range((int)minMaxSize.x, (int)(minMaxSize.y + 1));
        NewBlock.GetComponent<Block>().GenerateTiles(NewBlock.transform, blockSize);
    }
    
    void Start()
    {
        GenerateBlock();
    }

}
