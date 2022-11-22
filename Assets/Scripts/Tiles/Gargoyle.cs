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
    public Gargoyle(Sprite[] options, Transform parentTransform, Vector3 pos)
    {
        this.TileObject = new GameObject("Tile");
        this.TileObject.AddComponent<SpriteRenderer>();
        this.TileObject.transform.position = pos;
        this.TileObject.transform.rotation = Quaternion.identity;
        this.TileObject.transform.localScale *= GridManager.Instance.GridUnit;
        this.TileObject.transform.parent = parentTransform;
        this.TileObject.GetComponent<SpriteRenderer>().sprite = options[Random.Range(0, options.Length)];
    }

    public int CalculateScore()
    {
        return scoreWorth;
    }
}
