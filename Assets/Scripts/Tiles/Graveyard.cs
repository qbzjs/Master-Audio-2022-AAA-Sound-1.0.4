using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Graveyard : ITile
{
    [SerializeField] private int scoreWorthAdjacent = 1;
    public int adjacentDestroyed = 0;

    public Graveyard(Sprite art, Transform parentTransform, Vector3 pos)
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
        return scoreWorth + adjacentDestroyed * scoreWorthAdjacent;
    }

    public string Type()
    {
        return "GR";
    }
}
