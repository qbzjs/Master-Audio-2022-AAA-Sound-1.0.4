using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    public GameObject bloodParticles, bloodGlow;

    public void InstantiateBloodParticles(Transform parent)
    {
        Instantiate(bloodParticles, parent);
        bloodParticles.transform.localPosition = Vector3.zero;
    }
    
    public void InstantiateBloodGlow(Transform parent)
    {
        Instantiate(bloodGlow, parent);
        bloodParticles.transform.localPosition = Vector3.zero;
    }

}
