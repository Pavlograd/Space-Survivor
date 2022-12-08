using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] ParticleSystem m_prefab_explosion;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] protected SFXsManager m_sFXsManager;

    [Header("Parameter")]
    public int m_life;
    public float m_move_speed = 5;
    public int m_score = 100;

    [Header("Variables")]
    protected Player m_player;

    //------------------------------------------------------------------------------

    public virtual void Start()
    {
        m_player = Player.Instance;
    }

    private void DeleteObject()
    {
        StageLoop.Instance.UpdateRemainingEnemies(-1);

        GameObject.Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        // Move to player
        transform.position += (m_player.transform.position - transform.position).normalized * m_move_speed * Time.deltaTime;

        // Look at player
        transform.rotation = GetRotationForPlayer();
    }

    // Get the correct rotation to face player
    protected Quaternion GetRotationForPlayer()
    {
        Vector3 direction = m_player.transform.position - transform.position;
        float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0.0f, 0.0f, rotationZ - 90f);
    }

    //------------------------------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {
        if (m_life <= 0) return; // Prevent calling when multiple bullets hit at the same time

        switch (other.transform.tag)
        {
            case "PlayerBullet":
                DestroyByPlayer(other.transform.GetComponent<Bullet>());
                break;
            case "Player":
                other.transform.GetComponent<PlayerLife>().HitPlayer(1);
                if (Hit()) ExploseObject();
                break;
            default:
                // Nothing to do with that object
                break;
        }
    }

    void DestroyByPlayer(Bullet a_player_bullet)
    {
        //delete bullet
        if (a_player_bullet)
        {
            a_player_bullet.DeleteObject();
        }

        if (Hit())
        {
            //add score
            if (StageLoop.Instance)
            {
                StageLoop.Instance.AddScore(m_score);
            }

            ExploseObject();
        }
    }

    // Destroy and create an explosion
    void ExploseObject()
    {
        // Create explosion
        Instantiate(m_prefab_explosion, transform.position, Quaternion.identity, transform.parent);

        //delete self
        DeleteObject();
    }

    // Return true if enemy life dropped to zero
    bool Hit()
    {
        m_life--;

        // Relaunch Coroutine
        if (m_life > 0)
        {
            StopCoroutine(HitCoroutine());
            StartCoroutine(HitCoroutine());
        }

        // Audio
        m_sFXsManager.Play("Hit");

        return m_life <= 0;
    }

    private IEnumerator HitCoroutine()
    {
        float timer = 1f;

        while (timer > 0f)
        {
            meshRenderer.material.color = meshRenderer.material.color == Color.white ? Color.clear : Color.white;
            yield return new WaitForSeconds(0.1f);
            timer -= 0.1f;
        }
        meshRenderer.material.color = Color.white;
    }
}
