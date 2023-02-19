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
    [SerializeField] private List<AddTileButton> addTileButtons, removeTileButtons;

    
    public void PopulateUpgrades()
    {
        
        for (int i = 0; i < addTileButtons.Count; i++)
        {
            PopulateTileButton(addTileButtons[i], true);
        }
        
        for (int i = 0; i < removeTileButtons.Count; i++)
        {
            PopulateTileButton(removeTileButtons[i], false);
        }
        
    }

    private void PopulateTileButton(AddTileButton toPopulate, bool isAddTile)
    {
        float spin = Random.Range(0f, 1f);
        Color borderColor = Color.white;
        string tileName;

        if (isAddTile)
        {
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
        }
        else
        {
            tileName = DeckManager.Instance.GetRandomCard();
        }
        
        toPopulate.SetValues(tileName, borderColor);
        
    }

    public void AcceptDeal()
    {
        string toAdd = "", toRemove = "";

        foreach (AddTileButton addTileButton in removeTileButtons)
        {
            if (addTileButton.Selected)
            {
                toRemove = addTileButton.card.CardName;
            }
        }
        foreach (AddTileButton addTileButton in addTileButtons)
        {
            if (addTileButton.Selected)
            {
                toAdd = addTileButton.card.CardName;
            }
        }
        DeckManager.Instance.AddToDeck(toAdd);
        DeckManager.Instance.Remove(toRemove);
    }
    
}