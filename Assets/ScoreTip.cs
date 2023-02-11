using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform)),RequireComponent(typeof(CanvasGroup))]
public class ScoreTip : MonoBehaviour
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI scoreText;
    
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
    
    public void Show(ITile tile)
    {
        Score body = tile.CalculateScore();
        canvasGroup.alpha = 1;
        scoreText.text = body.score.ToString();
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
