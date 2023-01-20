using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Gargoyle : Wasteland
{
    [SerializeField] new protected int scoreWorth = 2;

    public Gargoyle(Sprite art, Transform parentTransform, Vector3 pos)

    {
        this.TileObject = new GameObject("Tile");
        this.TileObject.AddComponent<SpriteRenderer>();
        this.TileObject.transform.position = pos;
        this.TileObject.transform.rotation = Quaternion.identity;
        this.TileObject.transform.parent = parentTransform;
        this.TileObject.GetComponent<SpriteRenderer>().sprite = art;
    }

    public override int CalculateScore()
    {
        return scoreWorth;
    }

    public override string Type()
    {
        return "GA";
    }

    public string ShowCalculation()
    {
        var description = "Point Value: " + scoreWorth;
        return description;
    }
    
}
