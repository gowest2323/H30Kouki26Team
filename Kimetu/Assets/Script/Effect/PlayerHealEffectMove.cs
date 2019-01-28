using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealEffectMove : MonoBehaviour
{
    ParticleSystem particle;

    // Use this for initialization
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        Transform target = particle.GetComponentInParent<Transform>();
        Vector3 axis = target.transform.TransformDirection(Vector3.forward);
        particle.transform.RotateAround(target.position, axis, 5.0f);
    }
}
