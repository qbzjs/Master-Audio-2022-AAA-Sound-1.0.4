using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class TweenManager : Singleton<TweenManager>
{
    [Range(0, 2)]
    [SerializeField] private float cameraShakeAmount, cameraShakeTime, blockUpAmount, blockAnimationTime;

    [SerializeField] private AnimationCurve blockCurve;
    
    /*
    [Button()]
    public void TestPlaceBlock()
    {
        PlaceBlock(FindObjectOfType<Block>().gameObject);
    }
    */
    
    
    public void PlaceBlock(GameObject ob, Action CB)
    {
        foreach (var sp in ob.GetComponentsInChildren<SpriteRenderer>())
        {
            sp.sortingOrder = 20;
        }

        LeanTween.moveLocalY(ob, ob.transform.localPosition.y + blockUpAmount, blockAnimationTime)
            .setEase(blockCurve)
            .setOnComplete(() =>
            {
                ParticleManager.Instance.PlayParticlesAt(ob.transform.position);
                Shake();
                CB();        
                foreach (var sp in ob.GetComponentsInChildren<SpriteRenderer>())
                {
                    sp.sortingOrder = -100;
                }

            });
    }

    [Button()]
    public void Shake()
    {
        LeanTween.moveX(Camera.main.gameObject, Camera.main.transform.position.x + cameraShakeAmount, cameraShakeTime)
            .setEase(LeanTweenType.easeShake);
    }
}
