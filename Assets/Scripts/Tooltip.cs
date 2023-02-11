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
    [SerializeField] private TextMeshProUGUI titleText, descriptionText, calculationText, tagsText;
    
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
    public void Show(ITile tile)
    {
        Tag[] tags = tile.GetTags();
        Score body = tile.CalculateScore();
        string description = tile.GetDescription();
        string title = tile.GetType().FullName;

        canvasGroup.alpha = 1;
        string tags_string = "";
        foreach (Tag tag in tags){
            if (tag != Tag.Null){
                string t_str =  "#";
                t_str += tag.ToString();
                tags_string += t_str;
            }
        }
        titleText.text = title;
        descriptionText.text = description;
        calculationText.text =  body.explanation + $" = {body.score}";
        tagsText.text = tags_string;
    }

    /// <summary>
    /// Turns the tooltip invisible
    /// </summary>
    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
