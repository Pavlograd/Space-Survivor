using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : StrongEnemy
{
    public override void Start()
    {
        m_shoot = false;
        base.Start();

        // Launch audio for the boss
        m_sFXsManager.Play("Boss");

        Invoke("StartShots", 2f);
    }

    void StartShots()
    {
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            SpawnBullet(Vector3.left);
            SpawnBullet(Vector3.right);

            // Audio
            m_sFXsManager.Play("Shot");

            yield return new WaitForSeconds(m_reload_time);
        }
    }

    void SpawnBullet(Vector3 offset)
    {
        Bullet bullet = Instantiate(m_prefab_enemy_bullet, transform.position, GetRotationForPlayer(), transform);

        bullet.transform.localPosition += offset;

        bullet.transform.parent = transform.parent;

        // Calculation direction between ship and mouse
        // Could be also done using GetDirectionWithRotation
        bullet.m_direction = (m_player.transform.position - transform.position).normalized;
    }
}
