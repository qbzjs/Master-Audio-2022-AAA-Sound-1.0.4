using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile 
{
    public GameObject TileObject;
    
    public Tile(){
        
    }
    public Tile(Sprite[] options, Vector3 pos) 
    {
        this.TileObject = new GameObject("Tile");
        int i = Random.Range(0, options.Length);
        this.TileObject.AddComponent<SpriteRenderer>();
        this.TileObject.transform.position = pos;
        this.TileObject.transform.rotation = Quaternion.identity;
        this.TileObject.GetComponent<SpriteRenderer>().sprite = options[i];
    }
}
