﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{

    public string GetDescription();
    public string GetCardRefName();

    public bool HighlightPredicate(ITile otherTile);
    public Tag[] GetTags();

    public GameObject TileObject { get; set; }

    public Vector3 LocalPosition();

    public Vector3 Position();

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

    public bool HasEffect(Effect toCheck);

    public int xPos { get; set; }
    public int yPos { get; set; }
    public Score TileScore { get; set; }
    public Score CalculateScore();
}


