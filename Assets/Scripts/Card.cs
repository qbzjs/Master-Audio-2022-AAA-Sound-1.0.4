using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scripts;

public class Card : MonoBehaviour
{
    private string cardName, cardDescription, cardPoints, cardTags;
    [SerializeField] private TextMeshProUGUI titleText, descriptionText, tagsText, scoreText;
    [SerializeField] public Image cardArt; 

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
        Score body = tile.CalculateScore();
    
        CardName = tile.GetType().FullName;

        string description = tile.GetDescription();
        //description += "<br><br>";
        //description += "Score: ";
        //description += body.explanation;
        //description += $" = {body.score}";
        CardDescription = description;

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
}
