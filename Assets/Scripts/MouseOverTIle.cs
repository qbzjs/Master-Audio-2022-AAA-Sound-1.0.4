using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using UnityEngine.EventSystems;

public class MouseOverTile: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public ITile Tile;
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        StartCoroutine(HoverForSeconds());
    }
    
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        StopAllCoroutines();
        GameManager.Instance.Tooltip.Hide();
    }

    public IEnumerator HoverForSeconds()
    {
        yield return new WaitForSeconds(0.7f);
        if(!GameManager.Instance.dragging){
            GameManager.Instance.Tooltip.Show(Tile);
        }
        yield break;
    }


}