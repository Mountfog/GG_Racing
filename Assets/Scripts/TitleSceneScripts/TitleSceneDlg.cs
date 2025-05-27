using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneDlg : MonoBehaviour
{
    public Button m_btnStart = null;
    public Button m_btnExit = null;
    public Button m_btnOptions = null;
    public Button m_btnOptionsSave = null;
    public Button m_btnOptionsExit = null;
    public Slider m_sliderBGM = null;
    public Slider m_sliderSFX = null;
    public GameObject m_OptionsPanel = null;
    public GameObject m_msgExit = null;
    public Button m_msgYes = null;
    public Button m_msgNo = null;
    public Image m_fadeOutPanel = null;
    public AudioSource m_audioSourceSFX = null;
    public AudioSource m_audioSourceBGM = null;
    [System.Serializable]
    public struct StructSFX
    {
        public string name;
        public AudioClip clip;
    }
    public List<StructSFX> m_audioClips = new List<StructSFX>();
    public bool isCor = false;

    private void Awake()
    {
        m_OptionsPanel.SetActive(false);
        m_btnStart.onClick.AddListener(OnBtnClick_Start);
        m_btnExit.onClick.AddListener(OnBtnClick_Exit);
        m_btnOptions.onClick.AddListener(OnBtnClick_Options);
        m_msgYes.onClick.AddListener(OnClick_Yes);
        m_msgNo.onClick.AddListener(OnClick_No);
        m_btnOptionsSave.onClick.AddListener(OnBtnClick_OptionsSave);
        m_btnOptionsExit.onClick.AddListener(OnBtnClick_OptionsExit);
        m_sliderBGM.onValueChanged.AddListener((float f)=>OnValueChanged_BGM(f));
        m_fadeOutPanel.gameObject.SetActive(false);
        isCor = false;
        float bgm = PlayerPrefs.GetFloat("BgmVolume", 0.5f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        m_sliderBGM.value = bgm;
        m_sliderSFX.value = sfx;
        m_audioSourceBGM.volume = bgm;
        m_audioSourceSFX.volume = sfx;
    }
    public void OnBtnClick_Start()
    {
        m_btnExit.interactable = false;
        m_btnStart.interactable = false;
        StartCoroutine(LoadGame());
    }
    IEnumerator LoadGame()
    {
        PlaySFX("GameStart");
        yield return new WaitForSeconds(1f);
        m_fadeOutPanel.gameObject.SetActive(true);
        m_fadeOutPanel.color = new Color(0, 0, 0, 0);
        float curTime = Time.time;
        while(Time.time - curTime <= 2f)
        {
            float t = (Time.time - curTime) / 2f;
            m_fadeOutPanel.color = new Color(0, 0, 0, t);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GameScene");
        yield return null;
    }
    
    public void OnBtnClick_Exit()
    {
        PlaySFX("Confirm");
        m_msgExit.SetActive(true);
    }
    public void OnBtnClick_Options()
    {
        PlaySFX("Confirm");
        m_OptionsPanel.SetActive(true);
    }
    public void OnValueChanged_BGM(float f)
    {
        m_audioSourceBGM.volume = f;
    }
    public void OnBtnClick_OptionsSave()
    {
        PlaySFX("Confirm");
        PlayerPrefs.SetFloat("BgmVolume", m_sliderBGM.value);
        PlayerPrefs.SetFloat("SFXVolume", m_sliderSFX.value);
    }
    public void OnBtnClick_OptionsExit()
    {
        m_OptionsPanel.SetActive(false);
        float bgm = PlayerPrefs.GetFloat("BgmVolume", 1);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1);
        m_sliderBGM.value = bgm;
        m_sliderSFX.value = sfx;
        m_audioSourceBGM.volume = bgm;
        m_audioSourceSFX.volume = sfx;
        PlaySFX("Out");
    }
    public void PlaySFX(int k)
    {
        m_audioSourceSFX.PlayOneShot(m_audioClips[k].clip);
    }
    public void PlaySFX(string s)
    {
        AudioClip kclip = null;
        foreach (var c in m_audioClips)
        {
            if (c.name == s)
            {
                kclip = c.clip;
                break;
            }
        }
        if (kclip == null)
            return;
        m_audioSourceSFX.PlayOneShot(kclip);
    }

    public void OnClick_Yes()
    {
        PlaySFX("Confirm");
        m_msgExit.SetActive(false);
        Application.Quit();
    }
    public void OnClick_No()
    {
        PlaySFX("Out");
        m_msgExit.SetActive(false);
    }


}
