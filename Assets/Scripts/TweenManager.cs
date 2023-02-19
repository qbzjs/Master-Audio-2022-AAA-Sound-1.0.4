using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class TweenManager : Singleton<TweenManager>
{
    [Range(0, 2)]
    [SerializeField] private float cameraShakeAmount, cameraShakeTime, blockUpAmount, blockAnimationTime, emphasizeAmount, emphasizeTime, slideOnTime;

    [SerializeField] private AnimationCurve blockCurve;
    [SerializeField] private Vector3 slideOnDiff;

    
    /*
    [Button()]
    public void TestPlaceBlock()
    {
        PlaceBlock(FindObjectOfType<Block>().gameObject);
    }
    */
    public void SlideOnLocal(GameObject ob)
    {
        //LTSeq seq = new LTSeq();
        Vector3 endPos = ob.transform.localPosition;
        ob.transform.localPosition = endPos + slideOnDiff;
        LeanTween.moveLocal(ob, endPos, slideOnTime);
        CanvasGroup canvas = ob.GetComponent<CanvasGroup>();
        if (canvas == null)
        {
            canvas = ob.AddComponent<CanvasGroup>();
        }

        canvas.alpha = 0.5f;
        LeanTween.alphaCanvas(canvas, 1, slideOnTime);
    }
    
    public void PlaceBlock(GameObject ob, Action CB)
    {
        foreach (var sp in ob.GetComponentsInChildren<SpriteRenderer>())
        {
            sp.sortingOrder = 20;
        }

        Move(ob);
        Shake();
        CB();        
        foreach (var sp in ob.GetComponentsInChildren<SpriteRenderer>())
        {
            sp.sortingOrder = -100;
        }
    }

    [Button()]
    public void Shake()
    {
        LeanTween.moveY(Camera.main.gameObject, Camera.main.transform.position.x - cameraShakeAmount, cameraShakeTime)
            .setEase(LeanTweenType.easeShake).setDelay(0.1f);
    }

    public void Emphasize(GameObject ob)
    {
        LeanTween.scale(ob, Vector3.one * emphasizeAmount, emphasizeTime)
            .setEasePunch();
    }

    public void Move(GameObject ob)
    {
        LeanTween.moveLocalY(ob, ob.transform.localPosition.y + blockUpAmount, blockAnimationTime)
        .setEase(blockCurve);   
    }
}
