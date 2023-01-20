using System.Collections;
using System.Collections.Generic;
using System.IO;
using Scripts;
using UnityEngine;

public class Wasteland : ITile
{
    public GameObject TileObject { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }
    
    protected int scoreWorth = 0;
    private static string LOAD_ART_PREFIX = "Art/TileArt_";
    
    public Vector3 LocalPosition()
    {
        return TileObject.transform.localPosition;
    }

    public Vector3 Position()
    {
        return TileObject.transform.position;
    }

    public virtual bool Destructible()
    {
        return true;
    }

    public virtual string Type()
    {
        return "Wasteland";
    }
    public virtual int CalculateScore()
    {
        return scoreWorth;
    }

    protected void ConstructorHelper(Transform parentTransform, Vector3 pos, string tileName)
    {
        TileObject = new GameObject("Tile");
        TileObject.AddComponent<SpriteRenderer>();
        TileObject.transform.position = pos;
        TileObject.transform.rotation = Quaternion.identity;
        TileObject.transform.parent = parentTransform;
        TileObject.GetComponent<SpriteRenderer>().sprite = LoadArt(tileName);
    }

    protected Sprite LoadArt(string name)
    {
        string loadAt = LOAD_ART_PREFIX + name;
        //Debug.Log("attempting to load art: " + loadAt);
        Texture2D temp = Resources.Load<Texture2D>(loadAt);
        return Sprite.Create(temp, new Rect(0.0f, 0.0f, temp.width, temp.height), new Vector2(0.5f, 0.5f), temp.width);
    }
}
