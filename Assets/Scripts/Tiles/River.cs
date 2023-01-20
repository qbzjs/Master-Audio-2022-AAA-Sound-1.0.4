using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class River : Wasteland
{
    private bool blood = false;

    public River(Transform parentTransform, Vector3 pos)
    {
        ConstructorHelper(parentTransform, pos, "River");
    }
    public override bool Destructible()
    {
        return !blood;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>if it turned</returns>
    public bool CheckTurn()
    {
        if (blood) return false; //can't turn if already made of blood
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            string type = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y).Type();
            if (type == "BloodRiver" || type == "Fountain")
            {
                blood = true;
                TileObject.GetComponent<SpriteRenderer>().sprite = LoadArt("BloodRiver");
            }
        }
        return blood;
    }

    public override string Type()
    {
        if (blood)
        {
            return "BloodRiver";
        }
        return "River";
    }
    public string ShowCalculation()
    {
        var description = "Point Value: " + scoreWorth;
        return description;
    }   
}