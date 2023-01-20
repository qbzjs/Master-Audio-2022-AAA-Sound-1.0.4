using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Mansion : Wasteland
{
    [SerializeField] new protected int scoreWorth = 6;
    [SerializeField] protected int scoreWorthAdjacent = 0;

    public Mansion(Sprite art, Transform parentTransform, Vector3 pos)
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
        foreach (Vector2Int dir in Directions.Compass)
        {
            string type = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y).Type();
            if (type == "MA" || type == "TE")
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

    new public string Type()
    {
        return "MA";
    }
}
