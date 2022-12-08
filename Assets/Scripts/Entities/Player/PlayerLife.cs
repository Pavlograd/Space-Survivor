using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Player m_player;
    [SerializeField] MeshRenderer meshRenderer;

    [Header("Parameters")]
    public int m_life;
    public int m_max_life;
    // Time the player cannot be hit after being hit
    public float m_time_no_hit;

    [Header("Variables")]
    bool m_can_be_hit = true;

    void Start()
    {
        UpdateLifeUI();
    }

    // Player can be hit again
    void ResetHit()
    {
        m_can_be_hit = true;
    }

    // Coroutine to add effects when player get hit
    private IEnumerator HitCoroutine()
    {
        while (!m_can_be_hit)
        {
            meshRenderer.material.color = meshRenderer.material.color == Color.white ? Color.clear : Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        meshRenderer.material.color = Color.white;
    }

    public void AddLife(int life)
    {
        m_life = m_life + life > m_max_life ? m_max_life : m_life + life;

        UpdateLifeUI();
    }

    public void ResetLife()
    {
        m_life = m_max_life;

        UpdateLifeUI();
    }

    public void HitPlayer(int damage)
    {
        if (!m_can_be_hit) return; // Cannot be hit yet

        // XP Reset
        StageLoop.Instance.m_bonus_xp = 1;

        // UI
        UI_Manager.Instance.ChangeText("TextBonus", "Bonus x " + StageLoop.Instance.m_bonus_xp.ToString());

        m_life = m_life < damage ? 0 : m_life - damage;

        // Invincibility
        m_can_be_hit = false;
        StartCoroutine(HitCoroutine());
        Invoke("ResetHit", m_time_no_hit);

        if (m_life <= 0)
        {
            m_player.GameOver();
        }

        UpdateLifeUI();
    }

    // Update UI
    void UpdateLifeUI()
    {
        UI_Manager.Instance.ChangeText("TextPlayerLife", "Life " + m_life.ToString() + "/" + m_max_life.ToString());
    }
}
