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
    public Card cardRef;
    public bool HasCardRef;
 
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
         get => cardDescription;
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

        CardDescription = tile.GetDescription();
        descriptionText.text = CardDescription;

        CardPoints = body.score.ToString();

        Tag[] tags = tile.GetTags();
        CardTags = tagsToString(tags);
        foreach(var tooltip in toolTips){
            Destroy(tooltip);
        }
        toolTips.Clear();
        CreateCardToolTips();
        string cardRefName = tile.GetCardRefName();
        CreateCardRef(cardRefName);
        this.gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = UpgradeManager.Instance.FindColor(CardName);
    }
    private string tagsToString(Tag[] tags)
    {
        string tags_string = "";
        foreach (Tag tag in tags){
            if (tag != Tag.Null){
                string t_str =  "#";
                t_str += tag.ToString();
                tags_string += t_str;
            }
        }
        return tags_string;
    }
    public void CreateCardNewTile(string myTileName)
    {
        System.Type ClassType = System.Type.GetType(myTileName);

        ITile tile = TileFactory.CreateTile(ClassType, transform, Vector3.zero);
        Destroy(tile.TileObject);
        CreateCardExistingTile(tile);
        
    }
    public void CreateCardToolTips()
    {
        if(!tooltipParent)
        {
            return;
        }
        string description = cardDescription;
        List<string> keys = new List<string>(DeckManager.Instance.Keywords.Keys);
        foreach(string key in keys)
        {
            if (description.Contains(key))
            {
                GameObject newOb = Instantiate(DeckManager.Instance.tooltipPrefab, tooltipParent.transform);
                TextMeshProUGUI newTextMesh = newOb.GetComponentInChildren<TextMeshProUGUI>();
                newTextMesh.text = key + ": " + DeckManager.Instance.Keywords[key];
                toolTips.Add(newOb);
            }
        }
    }
    
    public void CreateCardRef(string cardRefName)
    {
        if(cardRefName == "")
        {
            HasCardRef = false;
            return;
        }
        cardRef.CreateCardNewTile(cardRefName);
        RectTransform rt = this.gameObject.GetComponent<RectTransform>();
        SetCardAnchoredSize(rt);
        HasCardRef = true;
        return;
    } 
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
       StartCoroutine(HoverForSeconds());
    }
    
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        StopAllCoroutines();
        tooltipParent.SetActive(false); 
        cardRef.gameObject.SetActive(false);
        TweenManager.Instance.ResetCard(gameObject);
    }  
    
    public void SetCardAnchoredSize(RectTransform _mRect)
    {
        _mRect.sizeDelta = new Vector2(0f, 0f);
        _mRect.anchoredPosition = new Vector2(0f, 0f);
        _mRect.anchorMin = new Vector2(0, 0);
        _mRect.anchorMax = new Vector2(1, 1);
        _mRect.pivot = new Vector2(0.5f, 0.5f);
    }

    public IEnumerator HoverForSeconds()
    {
        yield return new WaitForSeconds(0.75f);
        tooltipParent.SetActive(true);
        cardRef.gameObject.SetActive(HasCardRef);
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipParent.transform.GetComponent<RectTransform>());
        TweenManager.Instance.EmphasizeCard(gameObject);
        yield break;
    }

}
