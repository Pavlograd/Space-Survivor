using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Parameter")]
    public float m_move_speed = 1;
    public float m_max_speed = 1;

    [Header("Variables")]
    Camera m_camera;
    ParticleSystem.EmissionModule m_emission;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
