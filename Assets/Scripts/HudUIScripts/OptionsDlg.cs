using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsDlg : MonoBehaviour
{
    public Button m_btnOptionsSave = null;
    public Button m_btnOptionsExit = null;
    public Slider m_sliderBGM = null;
    public Slider m_sliderSFX = null;
    public AudioSource m_audioSourceBGM = null;
    public AudioSource m_audioSourceSFX = null;

    public void Initialize()
    {
        float bgm = PlayerPrefs.GetFloat("BgmVolume", 1);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1);
        m_sliderBGM.value = bgm;
        m_sliderSFX.value = sfx;
        m_audioSourceBGM.volume = bgm / 2f;
        m_audioSourceSFX.volume = sfx;
        m_btnOptionsSave.onClick.AddListener(OnBtnClick_OptionsSave);
        m_btnOptionsExit.onClick.AddListener(OnBtnClick_OptionsExit);
        m_sliderBGM.onValueChanged.AddListener((float f) => OnValueChanged_BGM(f));
    }
    public void OnValueChanged_BGM(float f)
    {
        m_audioSourceBGM.volume = f / 2f;
    }
    public void OnBtnClick_OptionsSave()
    {
        PlayerPrefs.SetFloat("BgmVolume", m_sliderBGM.value);
        PlayerPrefs.SetFloat("SFXVolume", m_sliderSFX.value);
    }
    public void OnBtnClick_OptionsExit()
    {
        gameObject.SetActive(false);
        float bgm = PlayerPrefs.GetFloat("BgmVolume", 1);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1);
        m_sliderBGM.value = bgm;
        m_sliderSFX.value = sfx;
        m_audioSourceBGM.volume = bgm / 2f;
        m_audioSourceSFX.volume = sfx;
    }
}
