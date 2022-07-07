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

    [Space, SerializeField] Text timerText;
    [SerializeField] float timeInMinutes;

    float currentTime;
    bool gameHasEnded;

    private void Awake()
    {
        currentTime = timeInMinutes * 60;
    }

    void Update()
    {
        if (gameHasEnded)
            return;

        UpdateTime();
    }
    void UpdateTime()
    {
        currentTime -= Time.deltaTime;

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timerText.text = time.ToString(@"mm\:ss");
        if (currentTime <= 0)
        {
            DisplayEndScreen();
        }
    }
    void DisplayEndScreen()
    {
        gameHasEnded = true;
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