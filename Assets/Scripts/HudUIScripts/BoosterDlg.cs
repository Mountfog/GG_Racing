using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterDlg : MonoBehaviour
{
    public Image m_boosterBG = null;
    public Image m_boosterSlider = null;
    public Text m_txtBooster = null;

    public void SetReadyState()
    {
        m_boosterSlider.fillAmount = 0;
        m_txtBooster.color = Color.red;
        m_boosterSlider.color = Color.red;
        m_txtBooster.text = "0.0%";
        ActivateUI(m_boosterBG.gameObject);
        ActivateUI(m_boosterSlider.gameObject);
        ActivateUI(m_txtBooster.gameObject);
    }
    public void SetGameState()
    {
        BoosterUpdate();
        ActivateUI(m_boosterBG.gameObject);
        ActivateUI(m_boosterSlider.gameObject);
        ActivateUI(m_txtBooster.gameObject);
    }
    public void SetResultState()
    {
        HideUI(m_boosterBG.gameObject);
        HideUI(m_boosterSlider.gameObject);
        HideUI(m_txtBooster.gameObject);
    }
    public void BoosterUpdate()
    {
        m_boosterSlider.fillAmount = GameMgr.Inst.ginfo.Booster / 100f;
        m_txtBooster.text = string.Format("{0:0.0}%",GameMgr.Inst.ginfo.Booster);
        if(GameMgr.Inst.ginfo.Booster == 100)
        {
            m_txtBooster.color = Color.cyan;
            m_boosterSlider.color = Color.cyan;
        }
        else if (GameMgr.Inst.ginfo.Booster >= 60)
        {
            m_txtBooster.color = Color.green;
            m_boosterSlider.color = Color.green;
        }
        else if (GameMgr.Inst.ginfo.Booster >= 30)
        {
            m_txtBooster.color = Color.yellow;
            m_boosterSlider.color = Color.yellow;
        }
        else
        {
            m_txtBooster.color = Color.red;
            m_boosterSlider.color = Color.red;
        }

    }
    public  void HideUI(GameObject go)
    {
        go.SetActive(false);
    }
    public  void ActivateUI(GameObject go)
    {
        go.SetActive(true);
    }

}
