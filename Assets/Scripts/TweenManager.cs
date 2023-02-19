using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class TweenManager : Singleton<TweenManager>
{
    [Range(0, 2)]
    [SerializeField] private float cameraShakeAmount, cameraShakeTime, blockUpAmount, blockAnimationTime, emphasizeAmount, emphasizeTime, slideOnTime;

    [SerializeField] private AnimationCurve blockCurve;
    [SerializeField] private Vector3 slideOnDiff;

    public GameObject cardBack, cardParticles;
    public Transform cardStart, cardStop, cardMiddle1, cardMiddle2;
    public float cardRandomness;
    
    /*
    [Button()]
    public void TestPlaceBlock()
    {
        PlaceBlock(FindObjectOfType<Block>().gameObject);
    }
    */

    public void DrawCard(Action callback = null)
    {
        GameObject card = Instantiate(cardBack, cardStart);
        Vector3[] positions = new []{cardStart.position, cardMiddle1.position, cardMiddle2.position, cardStop.position};
        for (int i = 1; i < positions.Length; i++)
        {
            positions[i] = positions[i] +
                           new Vector3(Random.Range(0, cardRandomness), Random.Range(0, cardRandomness), 0);
        }
        LTSpline spline = new LTSpline(new[]{positions[0], positions[0], positions[1], positions[2], positions[3], positions[3]});
        //spline.
        card.transform.localPosition = Vector3.zero;
        //card.transform.localScale = Vector3.zero;
        LeanTween.moveSpline(card, spline, 1f).setEaseInOutCirc();
        LeanTween.scale(card, Vector3.one * 1.5f, 1f).setEaseInOutCirc()
            .setOnComplete(() =>
            {
                if (callback != null)
                {
                    callback.Invoke();
                }
                Instantiate(cardParticles, card.transform.position, Quaternion.identity);
                Destroy(card, 0.01f);
            });
    }
    
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
