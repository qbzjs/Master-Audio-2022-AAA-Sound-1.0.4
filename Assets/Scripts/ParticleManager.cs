using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    public GameObject particles;


    public void PlayParticlesAt(Vector3 position)
    {
        Instantiate(particles, position, quaternion.identity);
    }
}
