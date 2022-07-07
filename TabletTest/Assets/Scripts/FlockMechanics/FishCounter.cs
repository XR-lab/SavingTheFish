using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishCounter : MonoBehaviour
{
    public static FishCounter fishCounter;
    [SerializeField] Text fishCountText;

    int flocks;
    int currentFishCount;

    private void Awake()
    {
        fishCounter = this;
    }

    private void Start()
    {
        flocks = FindObjectsOfType<Flock>().Length;
        currentFishCount = FindObjectOfType<Flock>().flockSize * flocks;
        fishCountText.text = GetCurrentFishCountAsString();
    }

    public void FishGotHooked()
    {
        currentFishCount--;
        fishCountText.text = GetCurrentFishCountAsString();
    }

    public string GetCurrentFishCountAsString()
    {
        return currentFishCount.ToString();
    }
}
