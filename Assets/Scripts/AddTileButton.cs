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
    public GameObject highlight;
    public Image border;
    private bool selected;

    public bool Selected
    {
        get
        {
            return selected;
        }
        set
        {
            selected = value;
            highlight.SetActive(selected);
        }
    }
    private string tileName;

    public void SetValues(string myTileName, Color newColor)
    {
        tileName = myTileName;
        border.color = newColor;
        card.CreateCardNewTile(myTileName);
        card.GetComponent<Image>().raycastTarget=true;
        DeckManager.Instance.CreateCardToolTips(card, card.transform);
    }

    public void AddMyTile()
    {
        DeckManager.Instance.AddToDeck(tileName);
    }
    
    

}
