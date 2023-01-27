using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scripts;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Wasteland : ITile
{
    public GameObject TileObject { get; set; }

    public List<Effect> ongoingEffects;

    public int xPos { get; set; }
    public int yPos { get; set; }
    public Score TileScore {get; set;}
    
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

    }

    public Wasteland(Transform parentTransform, Vector3 pos, string tilename)
    {
        ongoingEffects = new();
        TileObject = new GameObject("Tile");
        TileObject.AddComponent<SpriteRenderer>();
        TileObject.transform.position = pos;
        TileObject.transform.rotation = Quaternion.identity;
        TileObject.transform.parent = parentTransform;
        TileObject.GetComponent<SpriteRenderer>().sprite = LoadArt(tilename);
    }

    /// <summary>
    /// Most subclasses will need to override this. To be called when the tile
    ///     is placed on the grid.
    /// </summary>
    public virtual void WhenPlaced(){ }

    /// <summary>
    /// Override this if you need
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="aboutToBeDestroyed"></param>
    public virtual void WhenAnyDestroyed(int x, int y, ITile aboutToBeDestroyed)
    {
        
    }

    /// <summary>
    /// Adds effect to the list affecting this tile. Will merge stacks with
    ///     identical descriptions and not go above maxStacks.
    /// </summary>
    /// <param name="toAdd">effect to add to the list</param>
    public void AddEffect(Effect toAdd)
    {
        if (ongoingEffects.Any((value) => { return value.description.Equals(toAdd.description); }))
        {
            //If there is already a version of this effect, add it to a stack
            Effect priorEffect = ongoingEffects.FirstOrDefault((value) => { return value.description.Equals(toAdd.description); });
            priorEffect.stacks = Mathf.Min(priorEffect.stacks + toAdd.stacks, priorEffect.maxStacks);
            return;
        }
        
        ongoingEffects.Add(toAdd);
        ongoingEffects.Sort(); //sorts by effect order
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
    /// <returns>The score worth of the tile</returns>
    protected virtual Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

    /// <summary>
    /// Takes CalculateBaseScore (which is overriden by subclasses) and adds Effects to it. 
    /// </summary>
    /// <returns></returns>
    public Score CalculateScore()
    {
        TileScore = CalculateBaseScore();
        
        foreach (Effect effect in ongoingEffects)
        {
            TileScore = effect.modify(TileScore);
        }

        return TileScore;
    }

    public virtual void Observe(DefaultEvent e)
    {

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
