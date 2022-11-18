using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : ITile
{
    public GameObject TileObject { get; set; }
    [SerializeField] private int scoreWorth = 10;

    public Gargoyle(Sprite[] options, Vector3 pos)
    {
        this.TileObject = new GameObject("Tile");
        int i = Random.Range(0, options.Length);
        this.TileObject.AddComponent<SpriteRenderer>();
        this.TileObject.transform.position = pos;
        this.TileObject.transform.rotation = Quaternion.identity;
        this.TileObject.GetComponent<SpriteRenderer>().sprite = options[i];
    }

    public int CalculateScore()
    {
        return scoreWorth;
    }
}
