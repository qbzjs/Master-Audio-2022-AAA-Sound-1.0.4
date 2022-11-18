using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public Sprite[] TileOptions;
    public int MaxBlockSize;
    
    void GenerateBlock()
    {
        GameObject NewBlock; 
        NewBlock = new GameObject("NewBlock");
        NewBlock.transform.parent = transform;
        NewBlock.AddComponent<Block>();
        NewBlock.AddComponent<BoxCollider>();
        int blockSize = Random.Range(1, MaxBlockSize);
        NewBlock.GetComponent<Block>().GenerateTiles(NewBlock.transform, blockSize, TileOptions);
    }
    
    void Start()
    {
        GenerateBlock();
    }

    // Update is called once per frame
    void Update()
   {
        if (Input.GetKeyDown("space")){
            GenerateBlock();
    /*        if (NewBlock != null)
            {
                List<GameObject> tmpTiles = NewBlock.Tiles;
                Debug.Log(tmpTiles.Count);
                for (int i = 0; i < tmpTiles.Count; i++)
                {
                    GameObject.Destroy(tmpTiles[i]);
                }
                NewBlock.Tiles.Clear();
            }
            Vector3 currPos = transform.position;
            int blockSize = Random.Range(1, MaxBlockSize);
            NewBlock = new Block(currPos, blockSize, TileOptions); */
        }   
    }  
}
