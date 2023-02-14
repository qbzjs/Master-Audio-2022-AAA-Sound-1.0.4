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

    public void SetValues(string myTileName, Color newColor)
    {
        tileName = myTileName;
        Image cardImage = card.GetComponent<Image>();
        cardImage.color = newColor;
        Image tileImage = card.transform.GetChild(0).gameObject.GetComponent<Image>();
        tileImage.color = newColor;
        card.CreateCardNewTile(myTileName);
    }

    public void AddMyTile()
    {
        DeckManager.Instance.AddToDeck(tileName);
    }

}
