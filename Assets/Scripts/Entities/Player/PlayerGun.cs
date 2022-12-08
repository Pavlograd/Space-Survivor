using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [Header("Prefabs")]
    public Bullet m_prefab_player_bullet;

    [Header("Components")]
    [SerializeField] Player m_player;
    [SerializeField] SFXsManager m_sFXsManager;

    [Header("Parameters")]
    public float m_reload_time;

    [Header("Variables")]
    bool m_can_shoot = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    void ResetShoot()
    {
        m_can_shoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && m_can_shoot)
        {
            Bullet bullet = Instantiate(m_prefab_player_bullet, transform.position, transform.rotation, transform.parent);

            // Calculation direction between ship and mouse
            // Could be also done using GetDirectionWithRotation
            bullet.m_direction = (m_player.GetMousePosition() - transform.position).normalized;

            m_can_shoot = false;

            // Reload gun
            Invoke("ResetShoot", m_reload_time);

            // Audio
            m_sFXsManager.Play("Shot");
        }
    }
}
