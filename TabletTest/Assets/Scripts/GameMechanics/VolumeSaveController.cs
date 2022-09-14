using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VolumeSaveController : MonoBehaviour
{
    [SerializeField] private Slider m_VolumeSlider = null;
    [SerializeField] private Text m_VolumeTextUI = null;

    private void Start()
    {
        LoadValues();
    }

    public void VolumeSlider(float m_Volume)
    {
        m_VolumeTextUI.text = m_Volume.ToString("0.00");
    }

    public void SaveVolumeButton()
    {
        float m_VolumeValue = m_VolumeSlider.value;
        PlayerPrefs.SetFloat("VolumeValue", m_VolumeValue);
        LoadValues();

    }

    void LoadValues()
    {
        float m_VolumeValue = PlayerPrefs.GetFloat("VolumeValue");
        m_VolumeSlider.value = m_VolumeValue;
        AudioListener.volume = m_VolumeValue;
    }

}
