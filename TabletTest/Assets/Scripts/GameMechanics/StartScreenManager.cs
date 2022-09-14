using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] GameObject m_Settings;
    [SerializeField] GameObject m_StartScreen;
    [SerializeField] GameObject m_Rules;
    private void Awake()
    {
        Time.timeScale = 1f;
        m_StartScreen.SetActive(true);
        m_Settings.SetActive(false);
        m_Rules.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenSettings()
    {
        m_Settings.SetActive(true);
        m_StartScreen.SetActive(false);
    }
    public void CloseSettings()
    {
        m_Settings.SetActive(false);
        m_StartScreen.SetActive(true);
    }
    public void RulesOpen()
    {
        m_Rules.SetActive(true);
        m_StartScreen.SetActive(false);
    }
    public void RulesClose()
    {
        m_Rules.SetActive(false);
        m_StartScreen.SetActive(true);
    }
}
