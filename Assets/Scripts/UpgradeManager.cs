using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private FullTilePool tilePool;
    [SerializeField] private List<AddTileButton> addTileButtons;

    
    public void PopulateUpgrades()
    {
        
        for (int i = 0; i < addTileButtons.Count; i++)
        {
            PopulateTileButton(addTileButtons[i]);
        }
        
    }

    private void PopulateTileButton(AddTileButton toPopulate)
    {
        float spin = Random.Range(0f, 1f);
        Color borderColor = Color.black;
        string tileName;

        if (spin < tilePool.CommonWeight) 
        { 
            // common
            borderColor = tilePool.CommonColor;
            tileName = tilePool.Commons.PickRandom();
        } 
        else if (spin < tilePool.CommonWeight + tilePool.UncommonWeight) 
        { 
            // uncommon
            borderColor = tilePool.UncommonColor;
            tileName = tilePool.Uncommons.PickRandom();
        } 
        else if (spin < tilePool.CommonWeight + tilePool.UncommonWeight + tilePool.RareWeight) 
        { 
            // rare
            borderColor = tilePool.RareColor;
            tileName = tilePool.Rares.PickRandom();          
        }
        else 
        { 
            // legendary
            borderColor = tilePool.LegendaryColor;
            tileName = tilePool.Legendaries.PickRandom();
        }
        
        toPopulate.SetValues(tileName, "temp", "temp", borderColor);
        
    }
    
}