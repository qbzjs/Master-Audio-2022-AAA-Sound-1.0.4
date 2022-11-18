using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile 
{
    public GameObject TileObject;
    
    public Tile(){
        
    }
    public Tile(Sprite mySprite, Transform parentTransform, Vector3 tmpPos) 
    {
        this.TileObject = new GameObject("Tile");
        this.TileObject.AddComponent<SpriteRenderer>();
        this.TileObject.transform.position = tmpPos;
        this.TileObject.transform.rotation = Quaternion.identity;
        this.TileObject.transform.localScale *= GridManager.Instance.GridUnit;
        this.TileObject.transform.parent = parentTransform;
        this.TileObject.GetComponent<SpriteRenderer>().sprite = mySprite;
    }
}
