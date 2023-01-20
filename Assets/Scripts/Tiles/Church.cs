using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Church : Wasteland
{
    [SerializeField] protected int scoreWorthAdjacent = 3;

    public Church(Sprite art, Transform parentTransform, Vector3 pos)
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

    new public string Type()
    {
        return "CH";
    }

    public string ShowCalculation()
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
        var description = "Point Value: " + scoreWorth + " Points from Adjacent Wings: " + (adjacentWings * scoreWorthAdjacent);
        return description;
    }
    
}
