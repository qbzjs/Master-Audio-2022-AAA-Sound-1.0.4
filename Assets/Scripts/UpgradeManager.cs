using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class UpgradeManager : Singleton<UpgradeManager>
{
    private List<int> upgradeIDs;
    [SerializeField] private List<UpgradeInfo> upgrades;
    [SerializeField] private List<TextMeshProUGUI> upgradeTexts;


    private void Awake()
    {
        upgradeIDs = new();
    }
    
    public void PopulateUpgrades()
    {
        upgradeIDs.Clear();
        for (int i = 0; i < upgradeTexts.Count; i++)
        {
            upgradeIDs.Add(Random.Range(0, upgrades.Count));
            upgradeTexts[i].text = upgrades[upgradeIDs[i]].displayText;
        }
        
    }
    
    public void Upgrade(int buttonID)
    {
        upgrades[upgradeIDs[buttonID]].effect.Invoke();
    }
}

[Serializable] struct UpgradeInfo
{
    public string displayText;
    public UnityEvent effect;
}