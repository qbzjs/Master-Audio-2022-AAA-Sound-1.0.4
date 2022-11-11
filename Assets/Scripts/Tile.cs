using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Sprite[] TileOptions;
    public SpriteRenderer tilePrefab;

    public void Initialize() {
        int i = Random.Range(0, Tiles.Length);

        // Spawn Group at current Position
        tilePrefab = Instantiate(tilePrefab,
                    transform.position,
                    Quaternion.identity);
        tilePrefab.sprite = TileOptions[i]; 
    }
}