using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddTileButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
            if (GameManager.Instance.DeckScreenActive) return;
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
        card.tooltipParent.SetActive(false);
        card.cardRef.gameObject.SetActive(false);

    }

    public void AddMyTile()
    {
        DeckManager.Instance.AddToDeck(tileName);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.Instance.DeckScreenActive) return;
        StartCoroutine(HoverForSeconds());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        card.tooltipParent.SetActive(false);
        card.cardRef.gameObject.SetActive(false);
    }
    
    public IEnumerator HoverForSeconds()
    {
        yield return new WaitForSeconds(0.75f);
        card.tooltipParent.SetActive(true);
        card.cardRef.gameObject.SetActive(card.HasCardRef);
        LayoutRebuilder.ForceRebuildLayoutImmediate(card.tooltipParent.transform.GetComponent<RectTransform>());
        yield break;
    }
}