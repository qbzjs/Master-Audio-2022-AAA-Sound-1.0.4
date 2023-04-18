using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform)),RequireComponent(typeof(CanvasGroup))]
public class Tooltip : MonoBehaviour
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public Card card;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
    }
    
    /// <summary>
    /// Makes tooltip visible, sets the title and body (form will be scoreCalculation = scoreValue)
    /// </summary>
    /// <param name="title">title of the tooltip</param>
    /// <param name="body">Score to display in the body</param>
    public void Show(ITile tile)
    {
        canvasGroup.alpha = 1;
        card.CreateCardExistingTile(tile);
        TweenManager.Instance.ShowCard(card.gameObject);
        card.tooltipParent.SetActive(true);
        if(card.HasCardRef){
            TweenManager.Instance.ShowCardRef(card.cardRef.gameObject, card.transform.position);
        }else{
            card.cardRef.gameObject.SetActive(false);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(card.tooltipParent.transform.GetComponent<RectTransform>());
        TweenManager.Instance.EmphasizeTooltips(card.toolTips);
    }

    /// <summary>
    /// Turns the tooltip invisible
    /// </summary>
    public void Hide()
    {
        TweenManager.Instance.HideToolTips(card.toolTips);
        canvasGroup.alpha = 0;
    }
}
