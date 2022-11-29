using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Church : ITile
{
    public GameObject TileObject { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }
    [SerializeField] private int scoreWorth = 0;
    [SerializeField] private int scoreWorthAdjacent = 3;

    public Church(int x, int y)
    {
        this.xPos = x;
        this.yPos = y;
    }

    public Church(Sprite art, Transform parentTransform, Vector3 pos)
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
        int adjacentWings = 0;

        if (GridManager.Instance.GetTile(xPos + 1, yPos).Type() == "WI")
        {
            adjacentWings++;
        }

        if (GridManager.Instance.GetTile(xPos, yPos - 1).Type() == "WI")
        {
            adjacentWings++;
        }

        if (GridManager.Instance.GetTile(xPos - 1, yPos).Type() == "WI")
        {
            adjacentWings++;
        }

        if (GridManager.Instance.GetTile(xPos, yPos + 1).Type() == "WI")
        {
            adjacentWings++;
        }

        return scoreWorth + adjacentWings * scoreWorthAdjacent;
    }

    public string Type()
    {
        return "CH";
    }
}
