using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : ITile
{
    public GameObject TileObject { get; set; }
    [SerializeField] private int scoreWorth = 10;
    public Gargoyle()
    {
        
    }
    public Gargoyle(Sprite[] options, Vector3 pos)
    {
        this.TileObject = new GameObject("Tile");
        this.TileObject.AddComponent<SpriteRenderer>();
        this.TileObject.transform.position = tmpPos;
        this.TileObject.transform.rotation = Quaternion.identity;
        this.TileObject.transform.localScale *= GridManager.Instance.GridUnit;
        this.TileObject.transform.parent = parentTransform;
        this.TileObject.GetComponent<SpriteRenderer>().sprite = mySprite;
    }

    public int CalculateScore()
    {
        return scoreWorth;
    }
}
