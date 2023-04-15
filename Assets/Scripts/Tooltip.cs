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
        card.tooltipParent.SetActive(true);
        card.cardRef.gameObject.SetActive(card.HasCardRef);
        LayoutRebuilder.ForceRebuildLayoutImmediate(card.tooltipParent.transform.GetComponent<RectTransform>());
    }

    /// <summary>
    /// Turns the tooltip invisible
    /// </summary>
    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
