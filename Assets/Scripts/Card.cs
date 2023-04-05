using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scripts;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string cardName, cardDescription, cardPoints, cardTags;
    [SerializeField] public TextMeshProUGUI titleText, descriptionText, tagsText, scoreText;
    [SerializeField] public Image cardArt; 
    [SerializeField] public GameObject tooltipParent;
    public List<GameObject> toolTips;
    public List<GameObject> cardRefs;
 
    public string CardName
    {
        get => cardName;
        set
        {
            cardName = value;
            titleText.text = value;
            cardArt.sprite = ArtManager.LoadTileArt(value);
        }
    }

    public string CardDescription
    {
        set
        {
            cardDescription = value;
            descriptionText.text = value;
        }
    }
    
    public string CardTags
    {
        set
        {
            cardTags = value;
            tagsText.text = value;
        }
    }
    public string CardPoints
    {
        set
        {
            cardPoints = value;
            scoreText.text = value;
        }
    }
    public void CreateCardExistingTile(ITile tile)
    {
        if (toolTips!= null)
        {
            foreach (var tooltip in toolTips)
            {
                Destroy(tooltip.gameObject);
            }
            toolTips.Clear();
        }
        Score body = tile.CalculateScore();
    
        CardName = tile.GetType().FullName;

        string description = tile.GetDescription();
        descriptionText.text = description;

        CardPoints = body.score.ToString();

        Tag[] tags = tile.GetTags();
        string tags_string = "";
        foreach (Tag tag in tags){
            if (tag != Tag.Null){
                string t_str =  "#";
                t_str += tag.ToString();
                tags_string += t_str;
            }
        }
        CardTags = tags_string;  

    }
    public void CreateCardNewTile(string myTileName)
    {
        System.Type ClassType = System.Type.GetType(myTileName);

        ITile tile = TileFactory.CreateTile(ClassType, transform, Vector3.zero);
        Destroy(tile.TileObject);
        CreateCardExistingTile(tile);
        
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        tooltipParent.SetActive(true);
        foreach(var card in cardRefs)
        {
            card.SetActive(true);
        }
    }
    
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        tooltipParent.SetActive(false);
        foreach(var card in cardRefs)
        {
            card.SetActive(false);
        }
    }  

}
