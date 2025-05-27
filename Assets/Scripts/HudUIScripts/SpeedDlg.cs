using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpeedDlg : MonoBehaviour
{
    public Text m_txtSpeed = null;
    public Image m_imgNeedleIn = null;
    public Image m_imgNeedleOut = null;
    public Image m_imgSpeedometer = null;

    public void SetReadyState()
    {
        m_txtSpeed.text = "00.0km/s";
        float needleValue = 0;
        float needleValueIn = 0.5f + needleValue + 0.005f;
        float needleValueOut = 0.5f + needleValue - 0.005f;
        m_imgNeedleIn.fillAmount = (needleValueIn < 0) ? 0 : needleValueIn;
        m_imgNeedleOut.fillAmount = (needleValueOut > 1) ? 1 : needleValueOut;
        ActivateUI(m_txtSpeed.gameObject);
        ActivateUI(m_imgNeedleIn.gameObject);
        ActivateUI(m_imgNeedleOut.gameObject);
        ActivateUI(m_imgSpeedometer.gameObject);
    }
    public void SetGameState()
    {
        ActivateUI(m_txtSpeed.gameObject);
        ActivateUI(m_imgNeedleIn.gameObject);
        ActivateUI(m_imgNeedleOut.gameObject);
        ActivateUI(m_imgSpeedometer.gameObject);
    }
    public void SetResultState()
    {
        HideUI(m_txtSpeed.gameObject);
        HideUI(m_imgNeedleIn.gameObject);
        HideUI(m_imgNeedleOut.gameObject);
        HideUI(m_imgSpeedometer.gameObject);
    }
    public void SpeedUpdate()
    {
        float speed = GameMgr.Inst.gamescene.m_gameUI.m_player.curSpeed * 3;
        m_txtSpeed.text = string.Format("{0:00.0}km/s",speed);
        float needleValue = speed / 400f;
        float needleValueIn = 0.5f + needleValue + 0.005f;
        float needleValueOut = 0.5f + needleValue - 0.005f;
        m_imgNeedleIn.fillAmount = (needleValueIn < 0) ? 0 : needleValueIn;
        m_imgNeedleOut.fillAmount = (needleValueOut > 1) ? 1 : needleValueOut;
    }
    public void HideUI(GameObject go)
    {
        go.SetActive(false);
    }
    public void ActivateUI(GameObject go)
    {
        go.SetActive(true);
    }
}
