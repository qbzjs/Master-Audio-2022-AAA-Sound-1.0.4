using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isDown = false;
   
    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;

    }
}
