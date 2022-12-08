using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Parameters")]
    public Vector3 m_direction;
    public float m_move_speed = 5;
    public float m_life_time = 2;

    void Update()
    {
        m_life_time -= Time.deltaTime;

        if (m_life_time <= 0)
        {
            DeleteObject();
        }
    }

    void FixedUpdate()
    {
        transform.position += m_direction * m_move_speed * Time.deltaTime;
    }

    public void DeleteObject()
    {
        GameObject.Destroy(gameObject);
    }
}
