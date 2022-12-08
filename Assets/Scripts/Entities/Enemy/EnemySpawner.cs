using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Enemy SpawnPoint
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Variables")]
    Vector3 m_stage_dimensions;
    public Wave wave;

    //------------------------------------------------------------------------------

    public void StartRunning()
    {
        // Get the perimeter of the screen
        m_stage_dimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        StartCoroutine(MainCoroutine());
    }

    private IEnumerator MainCoroutine()
    {
        // Spawn all enemies in the wave
        foreach (WaveSequence item in wave.s_sequences)
        {
            //spawn enemy
            if (item.s_prefab_enemy)
            {
                for (int i = 0; i < item.s_number; i++)
                {
                    // To add later : manual position
                    Instantiate(item.s_prefab_enemy, CreateRandomPosition(), Quaternion.identity, transform.parent);
                }
            }

            // Wait for next sequence of the wave
            yield return new WaitForSeconds(item.s_delay);
        }
    }

    private Vector3 CreateRandomPosition()
    {
        Vector3 position = new Vector3();
        Vector3 directionFromCenter = new Vector3();

        position.x = Random.Range(-1f * m_stage_dimensions.x, m_stage_dimensions.x);
        position.y = Random.Range(-1f * m_stage_dimensions.y, m_stage_dimensions.y);

        directionFromCenter = position.normalized;

        if (position.x != 0f || position.y != 0f) // In case is at center
        {
            // Move enemy out of screen only using x
            // May try to also use y later
            while (position.x < m_stage_dimensions.x && position.x > -1f * m_stage_dimensions.x && position.y < m_stage_dimensions.y && position.y > -1f * m_stage_dimensions.y)
            {
                position += directionFromCenter;
            }
        }

        return position;
    }

    //------------------------------------------------------------------------------

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }
}
