using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : Wasteland
{
    [SerializeField] protected int scoreWorth = 2;

    public Gargoyle(int x, int y) : base(x, y) { }

    public Gargoyle(Sprite art, Transform parentTransform, Vector3 pos, int x, int y) : base(x, y)

    {
        this.TileObject = new GameObject("Tile");
        this.TileObject.AddComponent<SpriteRenderer>();
        this.TileObject.transform.position = pos;
        this.TileObject.transform.rotation = Quaternion.identity;
        this.TileObject.transform.parent = parentTransform;
        this.TileObject.GetComponent<SpriteRenderer>().sprite = art;
    }

    public int CalculateScore()
    {
        return scoreWorth;
    }

    public string Type()
    {
        return "GA";
    }
}
