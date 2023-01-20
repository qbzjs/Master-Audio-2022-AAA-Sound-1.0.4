using System.Collections;
using System.Collections.Generic;
using Scripts;
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

    new public int CalculateScore()
    {
        int adjacentTenements = 0;

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            string type = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y).Type();
            if (type == "TE")
            {
                adjacentTenements++;
            }
        }

        return scoreWorth + adjacentTenements * scoreWorthAdjacent;
    }

    new public string Type()
    {
        return "TE";
    }
    public string ShowCalculation()
    {
        int adjacentTenements = 0;

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            string type = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y).Type();
            if (type == "TE")
            {
                adjacentTenements++;
            }
        }
        var description = "Point Value: " + scoreWorth + " Points from Adjacent Tenements: " + (adjacentTenements * scoreWorthAdjacent);
        return description;
    }
}
