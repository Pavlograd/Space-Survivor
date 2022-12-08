using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongEnemy : Enemy
{
    [Header("Prefabs")]
    public Bullet m_prefab_enemy_bullet;

    [Header("Parameters")]
    public float m_reload_time;

    [Header("Variables")]
    protected bool m_shoot = true; // Value to say if can shoot at start


    public override void Start()
    {
        base.Start(); // runs the code from the base

        if (m_shoot)
        {
            StartCoroutine(ShootCoroutine());
        }
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            Bullet bullet = Instantiate(m_prefab_enemy_bullet, transform.position, GetRotationForPlayer(), transform.parent);

            // Calculation direction between ship and mouse
            // Could be also done using GetDirectionWithRotation
            bullet.m_direction = (m_player.transform.position - transform.position).normalized;

            // Audio
            m_sFXsManager.Play("Shot");

            yield return new WaitForSeconds(m_reload_time);
        }
    }
}
