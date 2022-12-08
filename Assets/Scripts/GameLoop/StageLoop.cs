using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Stage main loop
/// </summary>
public class StageLoop : MonoBehaviour
{
    #region static 
    static public StageLoop Instance { get; private set; }
    #endregion

    [Header("ScriptableObject")]
    [SerializeField] Waves waves;

    [Header("Layout")]
    public Transform m_stage_transform;
    public Text m_stage_score_text;

    [Header("Prefab")]
    public Player m_prefab_player;
    public EnemySpawner m_prefab_enemy_spawner;
    public GameObject m_prefab_border;

    [Header("Variables")]
    int m_current_wave = 0;
    int m_number_enemies = 0;
    //
    int m_game_score = 0;
    EnemySpawner spawner;
    Vector3 m_stage_dimensions;
    public int m_bonus_xp = 1;

    //------------------------------------------------------------------------------

    #region loop
    public void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond * System.DateTime.Now.Millisecond);

        StartCoroutine(StageCoroutine());
    }

    // Create borders of the map
    // To launch before spawning player
    void CreateBorders()
    {
        GameObject border = Instantiate(m_prefab_border, m_stage_dimensions, Quaternion.identity, transform.parent);

        border.transform.localScale = new Vector3(0.5f, m_stage_dimensions.y * 4, 100);

        border = Instantiate(m_prefab_border, -1 * m_stage_dimensions, Quaternion.identity, transform.parent);

        border.transform.localScale = new Vector3(0.5f, m_stage_dimensions.y * 4, 100);

        border = Instantiate(m_prefab_border, -1 * m_stage_dimensions, Quaternion.identity, transform.parent);

        border.transform.localScale = new Vector3(m_stage_dimensions.x * 4, 0.5f, 100);

        border = Instantiate(m_prefab_border, m_stage_dimensions, Quaternion.identity, transform.parent);

        border.transform.localScale = new Vector3(m_stage_dimensions.x * 4, 0.5f, 100);
    }

    /// <summary>
    /// stage loop
    /// </summary>
    private IEnumerator StageCoroutine()
    {
        SetupStage();

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //exit stage
                CleanupStage();
                SceneManager.LoadScene("MainMenu");
                yield break;
            }
            yield return null;
        }
    }
    #endregion

    // Pause game
    public void Pause()
    {
        Time.timeScale = 0f;
    }

    // Play game after pause
    public void Play()
    {
        Time.timeScale = 1f;
    }

    void ChooseSkill()
    {
        Pause();
        UI_Manager.Instance.ShowPanel("PanelSkills");
    }

    void EndChooseSkill()
    {
        UI_Manager.Instance.HidePanel("PanelSkills");
        Play();
        NextWave();
    }

    public void DoubleLife()
    {
        Player.Instance.GetComponent<PlayerLife>().m_max_life *= 2;
        Player.Instance.GetComponent<PlayerLife>().AddLife(Player.Instance.GetComponent<PlayerLife>().m_max_life);
        EndChooseSkill();
    }

    public void DoubleReloadTime()
    {
        Player.Instance.GetComponent<PlayerGun>().m_reload_time /= 2;
        EndChooseSkill();
    }

    public void DoubleSpeed()
    {
        Player.Instance.GetComponent<Player>().m_move_speed *= 2;
        Player.Instance.GetComponent<Player>().m_max_speed *= 2;
        EndChooseSkill();
    }

    void SetupStage()
    {
        Instance = this;

        m_game_score = 0;
        RefreshScore();

        // Get the perimeter of the screen
        m_stage_dimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        CreateBorders();

        //create player
        {
            Player player = Instantiate(m_prefab_player, m_stage_transform);

            if (player)
            {
                player.transform.position = Vector3.zero;
                player.StartRunning();
            }
        }

        //create enemy spawner
        {
            {
                spawner = Instantiate(m_prefab_enemy_spawner, m_stage_transform);

                if (spawner)
                {
                    spawner.transform.position = Vector3.zero;
                }
            }
        }

        NextWave();
    }

    void CleanupStage()
    {
        //delete all object in Stage
        {
            for (var n = 0; n < m_stage_transform.childCount; ++n)
            {
                Transform temp = m_stage_transform.GetChild(n);
                GameObject.Destroy(temp.gameObject);
            }
        }

        Instance = null;
    }

    //------------------------------------------------------------------------------

    public void AddScore(int a_value)
    {
        m_game_score += a_value * m_bonus_xp;
        m_bonus_xp++;

        // UI
        UI_Manager.Instance.ChangeText("TextBonus", "Bonus x " + m_bonus_xp.ToString());
        RefreshScore();
    }

    void RefreshScore()
    {
        if (m_stage_score_text)
        {
            m_stage_score_text.text = $"Score {m_game_score:0000000}";
        }
    }

    void End()
    {
        //CleanupStage();

        GameObject.Find("Music").GetComponent<SFXsManager>().Play("End");

        UI_Manager.Instance.ShowPanel("End");
        UI_Manager.Instance.ChangeText("TextScoreEnd", $"Your Score {m_game_score:0000000}");
    }

    public void Win()
    {
        End();
        UI_Manager.Instance.ChangeText("TextEnd", "YOU WON");
    }

    public void GameOver()
    {
        End();
        UI_Manager.Instance.ChangeText("TextEnd", "GAME OVER");
    }

    // Waves

    // Return false if was not able to pass to next wave
    bool NextWave()
    {
        // No more wave
        if (m_current_wave >= waves.waves.Count)
        {
            return false;
        }

        // UI
        UI_Manager.Instance.ShowPanel("PanelNextWave");

        // 3s + 0.1f to let the time for the user and UI to understand the situation
        Invoke("LaunchWave", 3.1f);

        return true;
    }

    // Function called at the end of a wave
    // To use if you want to change UI
    void EndOfWave()
    {
        if (m_current_wave >= waves.waves.Count)
        {
            Win();
        }
        else
        {
            // End of wave, the player can now choose a new skill
            ChooseSkill();
        }
    }

    // Function called at the beginning of a wave
    // To use if you want to change UI
    void LaunchWave()
    {
        // Calculation of the number of enemies

        Wave wave = waves.waves[m_current_wave];

        m_number_enemies = 0;

        foreach (WaveSequence item in wave.s_sequences)
        {
            m_number_enemies += item.s_number;
        }

        m_current_wave++;

        spawner.wave = wave;
        spawner.StartRunning();

        // UI
        UI_Manager.Instance.ChangeText("TextWave", "Wave " + m_current_wave.ToString());
        UpdateRemainingEnemies(0);

        // Reset life player to maximum
        Player.Instance.GetComponent<PlayerLife>().ResetLife();
    }

    // Add or remove number of enemies and update UI
    public void UpdateRemainingEnemies(int number)
    {
        m_number_enemies += number;

        // UI
        UI_Manager.Instance.ChangeText("TextRemaining", "Enemies remaining " + m_number_enemies.ToString());

        // No more enemies end the wave please
        if (m_number_enemies <= 0)
        {
            EndOfWave();
        }
    }
}
