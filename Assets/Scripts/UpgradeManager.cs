using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private FullTilePool tilePool;
    [SerializeField] private List<AddTileButton> addTileButtons, removeTileButtons;
    [SerializeField] private Color ownedColor;
    [SerializeField] private Button accept, reroll, decline;
    private int rerolls;

    /// <summary>
    /// Populates the upgrade buttons with names and border colors. Attempts to have no cards match each other
    /// To (hopefully) create more interesting choices. This includes tiles on either side (i.e. remove options
    /// not matching add options) 
    /// </summary>
    private void Update()
    {
        bool addSelected = false, removeSelected = false;  
        foreach (AddTileButton button in addTileButtons)
        {
            addSelected = addSelected || button.Selected;
        }
        foreach (AddTileButton button in removeTileButtons)
        {
            removeSelected = removeSelected || button.Selected;
        }

        accept.interactable = addSelected && removeSelected;
    }

    public void FirstTimeUpgrade()
    {
        rerolls = 3; //magic number, number of rerolls to start with
        reroll.interactable = true;
        reroll.GetComponentInChildren<TextMeshProUGUI>().text = $"Reroll ({rerolls} left)";
        PopulateUpgrades();
    }
    
    public void Reroll()
    {
        rerolls--;
        if (rerolls == 0)
        {
            reroll.GetComponentInChildren<TextMeshProUGUI>().text = $"No rerolls left";
            reroll.interactable = false;
        }
        else
        {
            reroll.GetComponentInChildren<TextMeshProUGUI>().text = $"Reroll ({rerolls} left)";
        }
        PopulateUpgrades();
    }

    public void PopulateUpgrades()
    {
        foreach (AddTileButton button in addTileButtons)
        {
            button.Selected = false;
        }
        foreach (AddTileButton button in removeTileButtons)
        {
            button.Selected = false;
        }

        Color borderColor = ownedColor;
        List<string> allTiles = new List<string>();
        string tileName = "";
        
        for (int i = 0; i < addTileButtons.Count; i++)
        {
            for (int j = 0; j < 10; j++) //try to prevent duplicates 10 times
            {
                tileName = GetAddName(ref borderColor);
                if (!allTiles.Contains(tileName))
                {
                    break;
                }
            }
            allTiles.Add(tileName);
            addTileButtons[i].SetValues(tileName, borderColor);
        }
        
        for (int i = 0; i < removeTileButtons.Count; i++)
        {
            for (int j = 0; j < 10; j++) //try to prevent duplicates 10 times
            {
                tileName = GetRemoveName();
                if (!allTiles.Contains(tileName))
                {
                    break;
                }

                if (j == 9)
                {
                    Debug.Log("didn't break");
                }
            }
            allTiles.Add(tileName);
            removeTileButtons[i].SetValues(tileName, ownedColor);
        }
        
    }

    private string GetRemoveName()
    {
        return DeckManager.Instance.GetRandomCard();
    }

    private string GetAddName(ref Color borderColor)
    {
        float spin = Random.Range(0f, 1f);

        if (spin < tilePool.CommonWeight) 
        { 
            // common
            borderColor = tilePool.CommonColor;
            return tilePool.Commons.PickRandom();
        } 
        else if (spin < tilePool.CommonWeight + tilePool.UncommonWeight) 
        { 
            // uncommon
            borderColor = tilePool.UncommonColor;
            return tilePool.Uncommons.PickRandom();
        } 
        else if (spin < tilePool.CommonWeight + tilePool.UncommonWeight + tilePool.RareWeight) 
        { 
            // rare
            borderColor = tilePool.RareColor;
            return tilePool.Rares.PickRandom();          
        }
        else 
        { 
            // legendary
            borderColor = tilePool.LegendaryColor;
            return tilePool.Legendaries.PickRandom();
        }
        
        
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