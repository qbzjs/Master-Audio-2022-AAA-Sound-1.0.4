using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Tenement : Wasteland
{
    [SerializeField] protected int scoreWorthAdjacent = 2;

    public Tenement(Transform parentTransform, Vector3 pos)
    {
        ConstructorHelper(parentTransform, pos, "Tenement");
    }

    public override int CalculateScore()
    {
        int adjacentTenements = 0;

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            string type = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y).Type();
            if (type == "Tenement")
            {
                adjacentTenements++;
            }
        }

        return scoreWorth + adjacentTenements * scoreWorthAdjacent;
    }

    public override string Type()
    {
        return "Tenement";
    }
    public string ShowCalculation()
    {
        int adjacentTenements = 0;

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            string type = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y).Type();
            if (type == "Tenement")
            {
                adjacentTenements++;
            }
        }
        var description = "Point Value: " + scoreWorth + " Points from Adjacent Tenements: " + (adjacentTenements * scoreWorthAdjacent);
        return description;
    }
}
