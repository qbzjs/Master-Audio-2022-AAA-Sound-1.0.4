using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Mansion : ITile
{
    public GameObject TileObject { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }
    [SerializeField] private int scoreWorth = 6;
    [SerializeField] private int scoreWorthAdjacent = 0;

    public Mansion(int x, int y)
    {
        this.xPos = x;
        this.yPos = y;
    }

    public Mansion(Sprite art, Transform parentTransform, Vector3 pos)
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
        int adjacentTenements = 0;
        foreach (Vector2Int dir in Directions.Compass)
        {
            char type = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y).Type();
            if (type == 'M' || type == 'T')
            {
                adjacentTenements++;
            }
        }

        if (adjacentTenements == 0)
        {
            return scoreWorth;
        }

        return scoreWorthAdjacent;
    }

    public char Type()
    {
        return 'M';
    }
}
