using System.Collections;
using System.Collections.Generic;
using System.IO;
using Scripts;
using UnityEngine;

public class Wasteland : ITile
{
    public GameObject TileObject { get; set; }

    public List<Effect> ongoingEffects;

    public int xPos { get; set; }
    public int yPos { get; set; }
    
    protected int scoreWorth = 0;
    //The file path (within the Resources folder) to the folder
    //  where we store the tile art
    private static string TILE_ART_FOLDER_PATH = "Art/";
    
    //The prefix we agree on for tile art files
    private static string TILE_ART_PREFIX = "TileArt_";

    public Vector3 LocalPosition()
    {
        return TileObject.transform.localPosition;
    }

    public Vector3 Position()
    {
        return TileObject.transform.position;
    }

    public Wasteland()
    {
        ongoingEffects = new();
    }
    
    /// <summary>
    /// Most subclasses will need to override this. To be called when the tile
    ///     is placed on the grid
    /// </summary>
    public virtual void WhenPlaced() { }
    
    public void AddEffect(Effect toAdd)
    {
        ongoingEffects.Add(toAdd);
        ongoingEffects.Sort((first, second) =>
        {
            if (first.order > second.order)
            {
                return 1;
            } else if (first.order == second.order)
            {
                return 0;
            } else //if (first.order < second.order)
            {
                return -1;
            }
        });
    }

    /// <summary>
    /// Most subclasses will not need to override this, returns true by default
    /// </summary>
    /// <returns>Whether or not this will be able to be built on top of</returns>
    public virtual bool Destructible()
    {
        return true;
    }

    /// <summary>
    /// Override this function!
    /// </summary>
    /// <returns>The name of the tile, for other things to refer to it</returns>
    public virtual string Type()
    {
        return "Wasteland";
    }
    
    /// <summary>
    /// Override this function!
    /// </summary>
    /// <returns>The score worth of the tile</returns>
    protected virtual Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

    public Score CalculateScore()
    {
        Score toReturn = CalculateBaseScore();
        if (ongoingEffects == null)
        {
            Debug.Log("ongoing effects is null in " + this);
        }
        foreach (Effect effect in ongoingEffects)
        {
            toReturn = effect.modify(toReturn);
        }

        return toReturn;
    }

    /// <summary>
    /// Takes care of 99% of what subclass constructors need to worry about. (Ezra) didn't seem like
    /// you could inherit constructors because of obscure C# reasons, if anyone has a cleaner idea about
    /// how to accomplish it, please do!
    /// </summary>
    /// <param name="parentTransform">The parent which TileObject will be instantiated under in the hierarchy</param>
    /// <param name="pos">The 3D position to instantiate the TileObject</param>
    /// <param name="tileName">Name of the art to load (minus prefix). e.g. Gargoyle</param>
    protected void ConstructorHelper(Transform parentTransform, Vector3 pos, string tileName)
    {
        ongoingEffects = new();
        TileObject = new GameObject("Tile");
        TileObject.AddComponent<SpriteRenderer>();
        TileObject.transform.position = pos;
        TileObject.transform.rotation = Quaternion.identity;
        TileObject.transform.parent = parentTransform;
        TileObject.GetComponent<SpriteRenderer>().sprite = LoadArt(tileName);
    }

    /// <summary>
    /// For Tiles to easily load sprites
    /// </summary>
    /// <param name="name">name of the art to load (minus prefix) e.g. Gargoyle</param>
    /// <returns>The sprite loaded</returns>
    protected Sprite LoadArt(string name)
    {
        string loadAt = TILE_ART_FOLDER_PATH + TILE_ART_PREFIX + name;
        //Debug.Log("attempting to load art: " + loadAt);
        Texture2D temp = Resources.Load<Texture2D>(loadAt);
        return Sprite.Create(temp, new Rect(0.0f, 0.0f, temp.width, temp.height), new Vector2(0.5f, 0.5f), temp.width);
    }
}
