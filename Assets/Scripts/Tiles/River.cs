using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class River : ITile
{
    public GameObject TileObject { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }
    [SerializeField] private int scoreWorth = 0;

    private bool blood = false;

    public River(int x, int y)
    {
        this.xPos = x;
        this.yPos = y;
    }

    public River(Sprite art, Transform parentTransform, Vector3 pos)
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
            if (type == "BR" || type == "FO")
            {
                blood = true;
                TileObject.GetComponent<SpriteRenderer>().sprite = ArtManager.Instance.bloodRiverArt;
            }
        }
        return blood;
    }

    public int CalculateScore()
    {
        return scoreWorth;
    }

    public string Type()
    {
        if (blood)
        {
            return "BR";
        }
        return "RI";
    }
    public string ShowCalculation()
    {
        var description = "Point Value: " + scoreWorth;
        return description;
    }   
}