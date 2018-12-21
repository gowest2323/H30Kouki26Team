using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealEffectMove : MonoBehaviour
{
    ParticleSystem particle;
    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[5000];
    public GameObject target_p;
    bool healstart;
    PlayerAction action;

    // Use this for initialization
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        healstart = false;
        action = new PlayerAction();
    }

    // Update is called once per frame
    void Update()
    {
        Transform target = particle.GetComponentInParent<Transform>();
        Vector3 axis = target.transform.TransformDirection(Vector3.forward);
        particle.transform.RotateAround(target.position, axis, 7.0f);

        int count = particle.GetParticles(particles);
        for (int n = 0; n < particles.Length; n++)
        {
            float remainTime = particles[n].remainingLifetime;
            if (action.CanPierce() == true)
            {
                particles[n].position = Vector3.Lerp(particles[n].position, target_p.transform.position, 1f - remainTime);
            }
            healstart = true;
        }
        particle.SetParticles(particles, count);
    }
}
