using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public Sprite[] TileOptions;
    public int MaxBlockSize;
    public Block NewBlock;
    
    void GenerateBlock()
    {
         Vector3 currPos = transform.position;
         int blockSize = Random.Range(1, MaxBlockSize);
         NewBlock = new Block(currPos, blockSize, TileOptions);
    }
    
    void Start()
    {
    //    GenerateBlock();
    }

    // Update is called once per frame
    void Update()
    {
    /*    if (Input.GetKeyDown("space")){
            if (newBlock != null)
            {
                List<Tile> tmpTiles = newBlock.Tiles;
                for (int i = 0; i < tmpTiles.Count; i++)
                {
                    GameObject.Destroy(tmpTiles[i].tileObject);
                }
                //newBlock.Tiles.Clear();
            }
            Vector3 currPos = transform.position;
            int blockSize = Random.Range(1, maxBlockSize);
            newBlock = new Block(currPos, blockSize, tileOptions);
        } */
    } 
}
