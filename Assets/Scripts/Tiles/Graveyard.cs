using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Graveyard : ITile
{
    public GameObject TileObject { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }
    [SerializeField] private int scoreWorth = 0;
    [SerializeField] private int scoreWorthAdjacent = 1;
    private int[] adjacentDestroyed = new int[4];

    public Graveyard(int x, int y)
    {
        this.xPos = x;
        this.yPos = y;
    }

    public Graveyard(Sprite art, Transform parentTransform, Vector3 pos)
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
        if (GridManager.Instance.GetTile(xPos + 1, yPos + 1).Type() != 'W')
        {
            adjacentDestroyed[0]++;
        }

        if (GridManager.Instance.GetTile(xPos + 1, yPos - 1).Type() != 'W')
        {
            adjacentDestroyed[1]++;
        }

        if (GridManager.Instance.GetTile(xPos - 1, yPos + 1).Type() != 'W')
        {
            adjacentDestroyed[2]++;
        }

        if (GridManager.Instance.GetTile(xPos - 1, yPos - 1).Type() != 'T')
        {
            adjacentDestroyed[3]++;
        }

        int adjacentDestroyedTotal = 0;

        foreach (var a in adjacentDestroyed)
        {
            adjacentDestroyedTotal += a;
        }

        return scoreWorth + adjacentDestroyedTotal * scoreWorthAdjacent;
    }

    public char Type()
    {
        return 'T';
    }
}
