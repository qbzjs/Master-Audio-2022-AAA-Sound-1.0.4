using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{
    //public static string Description { get; }
    //public static string PointDescription { get; }

    public string GetDescription();

    public GameObject TileObject { get; set; }
    
    public Vector3 LocalPosition();

    public Vector3 Position();

    public bool Destructible();

    public void Observe(DefaultEvent e);

    /// <summary>
    /// GameManager calls this when this ITile is placed. A good place to add Rules.
    /// </summary>
    public void WhenPlaced();

    /// <summary>
    /// GridManager calls this when any ITile is destroyed. Note: this is NOT when THIS is destroyed, when ANY OTHER
    ///     ITile is destroyed. This is so, for instance, graveyards can know when their neighbors are destroyed.
    /// </summary>
    /// <param name="x">x position of the tile</param>
    /// <param name="y">y position of the tile</param>
    /// <param name="aboutToBeDestroyed">The tile about to be destroyed</param>
    public void WhenAnyDestroyed(int x, int y, ITile aboutToBeDestroyed);
    
    /// <summary>
    /// Responsible for keeping track of effects added, Wasteland implements the functionality
    /// </summary>
    /// <param name="toAdd">Effect to add</param>
    public void AddEffect(Effect toAdd);

    public int xPos { get; set; }
    public int yPos { get; set; }
    public Score TileScore { get; set; }
    
  //  public string Type();
    public Score CalculateScore();
}


