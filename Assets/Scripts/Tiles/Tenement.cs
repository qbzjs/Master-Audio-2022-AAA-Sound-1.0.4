using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tenement : Wasteland
{
    [SerializeField] protected int scoreWorthAdjacent = 2;

    public Tenement(Sprite art, Transform parentTransform, Vector3 pos)
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
        int adjacentTenements = 0;

        foreach (Vector2Int dir in Directions.Compass)
        {
            string type = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y).Type();
            if (type == "TE")
            {
                adjacentTenements++;
            }
        }

        return scoreWorth + adjacentTenements * scoreWorthAdjacent;
    }

    public string Type()
    {
        return "TE";
    }
}
