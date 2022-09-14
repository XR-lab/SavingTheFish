using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] GameObject infoPanel;
    [SerializeField] GameObject endScreen;
    [SerializeField] Text fishLeftText;
    [SerializeField] Text fishHooksStoppedText;

    //[Space, SerializeField] Text timerText;
    [Space, SerializeField] Slider m_TimerSlider;
    [SerializeField] float m_TimeInMinutes;

    bool m_GameHasEnded;

    private void Awake()
    {
        m_TimerSlider.maxValue = m_TimeInMinutes * 60;
        m_TimerSlider.value = m_TimeInMinutes * 60;
        m_GameHasEnded = false;
    }

    void Update()
    {
        float time = m_TimeInMinutes * 60 - Time.timeSinceLevelLoad;

        if (time <= 0)
        {
            DisplayEndScreen();
            SaveTheFishData data = FindObjectOfType<SaveTheFishData>();
            if (data != null) { data.IsPlaying = false; }
        }
        else if (m_GameHasEnded == false)
        {
            m_TimerSlider.value = time;
            SaveTheFishData data = FindObjectOfType<SaveTheFishData>();
            if (data != null) { data.IsPlaying = true; }
        }
    }

    void DisplayEndScreen()
    {
        m_GameHasEnded = true;
        FindObjectOfType<FishnetIndicator>().GameEnded();

        infoPanel.SetActive(false);
        endScreen.SetActive(true);

        fishLeftText.text = FishCounter.fishCounter.GetCurrentFishCountAsString();
        fishHooksStoppedText.text = FishHookCounter.fishHookCounter.GetFishHooksStoppedAsString();

        Time.timeScale = 0f;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}