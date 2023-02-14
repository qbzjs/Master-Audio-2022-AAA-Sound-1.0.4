using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AddTileButton : MonoBehaviour
{
    public Card card;
    private string tileName;

    public void SetValues(string myTileName)
    {
        tileName = myTileName;
        card.CreateCardNewTile(myTileName);
    }

    public void AddMyTile()
    {
        DeckManager.Instance.AddToDeck(tileName);
    }

}
