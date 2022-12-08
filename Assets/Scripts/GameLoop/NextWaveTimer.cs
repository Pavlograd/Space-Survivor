using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextWaveTimer : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        int i = 3;
        yield return new WaitForSeconds(0.1f);

        while (i > 0)
        {
            UI_Manager.Instance.ChangeText("TextSecondsWave", i.ToString());
            i--;
            yield return new WaitForSeconds(1);
        }

        // Now hide
        gameObject.SetActive(false);
    }
}
