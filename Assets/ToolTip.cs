using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform)),RequireComponent(typeof(CanvasGroup))]
public class ToolTip : MonoBehaviour
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

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
    // Update is called once per frame
    void Update()
    {
        
    }
}
