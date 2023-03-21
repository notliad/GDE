using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgniteEspada : MonoBehaviour
{
    [SerializeField] ParticleSystem ignitingParticles;
    [SerializeField] ParticleSystem ignitedParticles;
    [SerializeField] MeshRenderer ticao;
    bool ignited = false;
    bool igniting = false;

    void FixedUpdate()
    {
        if (igniting)
        {
            if (ignitingParticles.time > 5.5)
            {
                ignitedParticles.Play();
                ignitingParticles.Stop();
                ignited = true;
                igniting = false;
            }
        }
        else if (ignited)
        {
            if (ignitedParticles.time > 14.9)
            {
                ignitedParticles.Stop();
                ignited = false;
            }
        }
    }

    public void Ignite()
    {
        ignitingParticles.Play();
        igniting = true;
    }

    public void SetTicao(bool _ticao)
    {
        ticao.enabled= _ticao;
    }
}
