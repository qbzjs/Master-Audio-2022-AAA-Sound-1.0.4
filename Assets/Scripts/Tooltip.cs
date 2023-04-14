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

        Vector2 topRightCorner = rectTransform.GetCorners().topRight;
        Vector2 topRightBoundaryPoint = DeckManager.Instance.topRightBoundary.GetCorners().topRight;
        Vector3 worldPosition = rectTransform.TransformPoint(rectTransform.rect.position);
        
        if (topRightCorner.x > topRightBoundaryPoint.x)
        {
            worldPosition.x += topRightBoundaryPoint.x - topRightCorner.x;
        }
        if (topRightCorner.y > topRightBoundaryPoint.y)
        {
            worldPosition.y += topRightBoundaryPoint.y - topRightCorner.y;
        }

        transform.position = worldPosition; 
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
        DeckManager.Instance.CreateCardToolTips(card);
        DeckManager.Instance.CreateCardRef(card);
        LayoutRebuilder.ForceRebuildLayoutImmediate(card.tooltipParent.transform.GetComponent<RectTransform>());
        if(card.cardRef)
        {
            card.cardRef.SetActive(true);
        }
    }

    /// <summary>
    /// Turns the tooltip invisible
    /// </summary>
    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
