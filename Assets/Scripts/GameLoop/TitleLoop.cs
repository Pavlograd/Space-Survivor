using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Title Screen Loop
/// </summary>
public class TitleLoop : MonoBehaviour
{
    [Header("Layout")]
    public Transform m_ui_title;

    //------------------------------------------------------------------------------

    private void Start()
    {
        //default start
        StartTitleLoop();
    }

    //
    #region loop
    public void StartTitleLoop()
    {
        StartCoroutine(TitleCoroutine());
    }

    /// <summary>
    /// Title loop
    /// </summary>
    private IEnumerator TitleCoroutine()
    {
        Debug.Log($"Start TitleCoroutine");

        SetupTitle();

        //waiting game start
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CleanupTitle();
                SceneManager.LoadScene("Game");
                //Start StageLoop
                yield break;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                CleanupTitle();
                yield break;
            }
            yield return null;
        }
    }
    #endregion

    //
    void SetupTitle()
    {
        m_ui_title.gameObject.SetActive(true);
    }

    void CleanupTitle()
    {
        m_ui_title.gameObject.SetActive(false);
    }
}
