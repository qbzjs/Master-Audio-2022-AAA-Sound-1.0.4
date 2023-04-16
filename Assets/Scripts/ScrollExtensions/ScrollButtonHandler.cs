using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollButtonHandler : MonoBehaviour
{
    private ScrollRect scrollRect;
    [SerializeField] private ScrollButton leftButton;
    [SerializeField] private ScrollButton rightButton;
    [SerializeField] private float scrollSpeed = 0.00001f;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        if(leftButton != null)
        {
            if(leftButton.isDown)
            {
                ScrollLeft();    
            }
        }
        if(rightButton != null)
        {
            if(rightButton.isDown)
            {
                ScrollRight();    
            }
        }
        
    }

    private void ScrollLeft()
    {
        if(scrollRect != null)
        {
            if(scrollRect.horizontalNormalizedPosition >= 0f)
            {
                scrollRect.horizontalNormalizedPosition -= scrollSpeed;
            }
        }
    }
    private void ScrollRight()
    {
        if(scrollRect != null)
        {
            if(scrollRect.horizontalNormalizedPosition <= 1f)
            {
                scrollRect.horizontalNormalizedPosition += scrollSpeed;
            }
        }
    }
}
