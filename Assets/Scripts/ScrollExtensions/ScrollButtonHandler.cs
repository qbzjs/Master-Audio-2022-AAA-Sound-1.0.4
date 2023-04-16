using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollButtonHandler : MonoBehaviour
{
    private ScrollRect scrollRect;
    [SerializeField] private ScrollButton downButton;
    [SerializeField] private ScrollButton upButton;
    [SerializeField] private float scrollSpeed;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        if(downButton != null)
        {
            if(downButton.isDown)
            {
                ScrollDown();    
            }
        }
        if(upButton != null)
        {
            if(upButton.isDown)
            {
                ScrollUp();    
            }
        }
        
    }

    private void ScrollDown()
    {
        if(scrollRect != null)
        {
            if(scrollRect.verticalNormalizedPosition >= 0f)
            {
                scrollRect.verticalNormalizedPosition -= scrollSpeed;
            }
        }
    }
    private void ScrollUp()
    {
        if(scrollRect != null)
        {
            if(scrollRect.verticalNormalizedPosition <= 1f)
            {
                scrollRect.verticalNormalizedPosition += scrollSpeed;
            }
        }
    }
}
