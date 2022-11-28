using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : ITile
{
    public GameObject TileObject { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }

    [SerializeField] private int scoreWorth = 2;

    public Gargoyle(int x, int y)
    {
        this.xPos = x;
        this.yPos = y;
    }

    public Gargoyle(Sprite art, Transform parentTransform, Vector3 pos)

    {
        this.TileObject = new GameObject("Tile");
        this.TileObject.AddComponent<SpriteRenderer>();
        this.TileObject.transform.position = pos;
        this.TileObject.transform.rotation = Quaternion.identity;
        this.TileObject.transform.parent = parentTransform;
        this.TileObject.GetComponent<SpriteRenderer>().sprite = art;
    }

    public Vector3 LocalPosition()
    {
        return TileObject.transform.localPosition;
    }

    public Vector3 Position()
    {
        return TileObject.transform.position;
    }

    public bool Destructible()
    {
        return true;
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
