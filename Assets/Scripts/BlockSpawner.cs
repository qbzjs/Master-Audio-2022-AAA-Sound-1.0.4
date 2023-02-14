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

    public Transform mawSpawner;

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

    public void GenerateMaw()
    {
        GameObject NewBlock; 
        NewBlock = new GameObject("Maw");
        NewBlock.transform.parent = mawSpawner;
        NewBlock.transform.localPosition = Vector3.zero;
        NewBlock.AddComponent<Block>();
        NewBlock.GetComponent<Block>().GenerateMaw(NewBlock.transform);
    }
    
    void Start()
    {
        GenerateBlock();
        GenerateMaw();
    }

}
