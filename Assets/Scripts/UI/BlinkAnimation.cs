using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkAnimation : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Text text;
    [SerializeField] Image image;

    [Header("Parameters")]
    [SerializeField] float m_timer = 0.2f;

    [Header("Variables")]
    Color defaultColorText;
    Color defaultColorImage;

    // Start is called before the first frame update
    void Start()
    {
        if (text)
        {
            defaultColorText = text.color;
        }
        if (image)
        {
            defaultColorImage = image.color;
        }
        StartCoroutine(MainCoroutine());
    }

    // Coroutine to add effects
    private IEnumerator MainCoroutine()
    {
        while (true)
        {
            if (text)
            {
                text.color = text.color == defaultColorText ? Color.white : defaultColorText;
            }
            if (image)
            {
                image.color = image.color == defaultColorImage ? Color.white : defaultColorImage;
            }
            yield return new WaitForSeconds(m_timer);
        }
    }
}
