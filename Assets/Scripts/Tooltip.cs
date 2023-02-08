using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform)),RequireComponent(typeof(CanvasGroup))]
public class Tooltip : MonoBehaviour
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI titleText, descriptionText;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        temp.z = 0;
        transform.position = temp;
    }
    
    /// <summary>
    /// Makes tooltip visible, sets the title and body (form will be scoreCalculation = scoreValue)
    /// </summary>
    /// <param name="title">title of the tooltip</param>
    /// <param name="body">Score to display in the body</param>
    public void Show(string title, Score body)
    {
        canvasGroup.alpha = 1;
        titleText.text =  body.explanation + $" = {body.score}";
        descriptionText.text = title;
    }

    /// <summary>
    /// Turns the tooltip invisible
    /// </summary>
    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
