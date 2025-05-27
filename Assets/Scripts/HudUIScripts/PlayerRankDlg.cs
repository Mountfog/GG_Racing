using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRankDlg : MonoBehaviour
{
    public Text m_txtRank = null;
    public Text m_txtLap = null;
    public Text m_txtLapAlarm = null;

    public void SetReadyState()
    {
        m_txtLap.text = "Lap (1/3)";
        ActivateUI(m_txtRank.gameObject);
        ActivateUI(m_txtLap.gameObject);
        HideUI(m_txtLapAlarm.gameObject);
        m_txtRank.text = "----";
    }
    public void SetGameState()
    {
        ActivateUI(m_txtRank.gameObject);
        ActivateUI(m_txtLap.gameObject);
        LapAlarm();
    }
    public void SetResultState()
    {
        HideUI(m_txtRank.gameObject);
        HideUI(m_txtLap.gameObject);
        HideUI(m_txtLapAlarm.gameObject);
    }
    public void HideUI(GameObject go)
    {
        go.SetActive(false);
    }
    public void ActivateUI(GameObject go)
    {
        go.SetActive(true);
    }
    public void LapAlarm()
    {
        if (!GameMgr.Inst.ginfo.IsGameEnded)
        {
            StartCoroutine(LapCor());
        }
    }
    IEnumerator LapCor()
    {
        int lap = GameMgr.Inst.ginfo.CurLap;
        if(lap == 4)
        {
            yield break;
        }
        if (lap == 3)
        {
            m_txtLapAlarm.text = "Last Lap";
        }
        else
        {
            m_txtLapAlarm.text = string.Format("Lap {0}", lap);
        }
        m_txtLapAlarm.color = Color.blue;
        ActivateUI(m_txtLapAlarm.gameObject);
        yield return new WaitForSeconds(0.5f);
        HideUI(m_txtLapAlarm.gameObject);
        yield return new WaitForSeconds(0.5f);
        ActivateUI(m_txtLapAlarm.gameObject);
        yield return new WaitForSeconds(0.5f);
        HideUI(m_txtLapAlarm.gameObject);
        yield return new WaitForSeconds(0.5f);
        ActivateUI(m_txtLapAlarm.gameObject);
        yield return new WaitForSeconds(0.5f);
        HideUI(m_txtLapAlarm.gameObject);
        yield return new WaitForSeconds(0.5f);
        yield return null;
    }
    public void RankUpdate()
    {
        if(GameMgr.Inst.ginfo.StageTime > 5)
        {
            int rank = GameMgr.Inst.ginfo.CurRank;

            if (rank == 1)
            {
                m_txtRank.text = "1st";
            }
            else if (rank == 2)
            {
                m_txtRank.text = "2nd";
            }
            else if (rank == 3)
            {
                m_txtRank.text = "3rd";
            }
            else
            {
                m_txtRank.text = string.Format("{0}th", rank);
            }
        }
    }
    public void LapUpdate()
    {
        int lap = GameMgr.Inst.ginfo.CurLap;
        m_txtLap.text = string.Format("Lap({0}/3)", lap);
    }
}
