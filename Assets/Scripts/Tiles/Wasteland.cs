using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scripts;
using UnityEngine;

public class Wasteland : ITile
{
    public virtual string GetDescription()
    {
        return "<i>A barren wasteland</i>";
    }

    public GameObject TileObject { get; set; }

    public List<Effect> ongoingEffects;

    public bool HasEffect(Effect toCheck)
    {
        foreach (var effect in ongoingEffects)
        {
            if (effect.description == toCheck.description)
            {
                return true;
            }
        }

        return false;
    }

    public int xPos { get; set; }
    public int yPos { get; set; }
    public string Type = "";
    public Score TileScore {get; set;}
    protected int scoreWorth = 0;

    public virtual bool HighlightPredicate(ITile otherTile)
    {
        return otherTile.GetType() == this.GetType();
    }

    public virtual Tag[] GetTags()
    {
        return new []{Tag.Null};
    }
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

    public Wasteland(Transform parentTransform, Vector3 pos)
    {
        ongoingEffects = new();
        TileObject = new GameObject("Tile");
        TileObject.AddComponent<SpriteRenderer>();
        TileObject.transform.position = pos;
        TileObject.transform.rotation = Quaternion.identity;
        TileObject.transform.parent = parentTransform;
        TileObject.AddComponent<BoxCollider>();
       // TileObject.AddComponent<MouseOverTile>().Tile = this;
        TileObject.GetComponent<SpriteRenderer>().sprite = ArtManager.LoadTileArt(GetType().ToString());
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
            //If there is already a version of this effect, add it to a stack (or maybe overwrite)
            Effect priorEffect = ongoingEffects.FirstOrDefault((value) => { return value.description.Equals(toAdd.description); });
            if (priorEffect.maxStacks == 1)
            {
                ongoingEffects.Remove(priorEffect);
                ongoingEffects.Add(toAdd);
            }
            priorEffect.stacks = Mathf.Min(priorEffect.stacks + toAdd.stacks, priorEffect.maxStacks);
            return;
        }
        
        ongoingEffects.Add(toAdd);
        ongoingEffects.Sort(); //sorts by effect order
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
        Score toReturn = CalculateBaseScore();
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


}
