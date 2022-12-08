using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    #region static 
    static public UI_Manager Instance { get; private set; }
    #endregion

    [Header("Panels")]
    [SerializeField] GameObject[] m_panels;

    [Header("Texts")]
    Text[] m_texts;

    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        Instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get all texts in the scene
        m_texts = Resources.FindObjectsOfTypeAll<Text>();
    }

    public void ChangeText(string name, string value)
    {
        string altText = "Text" + name; // In case you just change Money instead of TextMoney

        foreach (Text item in m_texts)
        {
            if (item.gameObject.name == name || item.gameObject.name == altText)
            {
                item.text = value;
                break;
            }
        }
    }

    public void ShowPanel(string name)
    {
        string altText = "Text" + name; // In case you just change Money instead of PanelMoney

        foreach (GameObject item in m_panels)
        {
            if (item.gameObject.name == name || item.gameObject.name == altText)
            {
                item.SetActive(true);
                break;
            }
        }
    }

    public void HidePanel(string name)
    {
        string altText = "Text" + name; // In case you just change Money instead of PanelMoney

        foreach (GameObject item in m_panels)
        {
            if (item.gameObject.name == name || item.gameObject.name == altText)
            {
                item.SetActive(false);
                break;
            }
        }
    }
}
