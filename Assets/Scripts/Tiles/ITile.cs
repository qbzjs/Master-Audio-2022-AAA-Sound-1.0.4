using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile 
{
    public GameObject TileObject { get; set; }
    [SerializeField] private int scoreWorth;

    public int calulate_score();
}


// interface the Tile
// subclasses/implementations
//   calculate score
//   
//   destroyed metric
//      get call in grid manager to get adjacent tiles
//