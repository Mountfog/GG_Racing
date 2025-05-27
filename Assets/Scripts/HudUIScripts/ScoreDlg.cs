using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDlg : MonoBehaviour
{
    public Text m_txtStage = null;
    public Text m_txtTime = null;

    public void SetReadyState()
    {
        ActivateUI(m_txtStage.gameObject);
        ActivateUI(m_txtTime.gameObject);
        m_txtStage.text = string.Format("Stage {0}",GameMgr.Inst.ginfo.CurStage);
    }

    public void SetResultState()
    {
        HideUI(m_txtStage.gameObject);
        HideUI(m_txtTime.gameObject);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameMgr.Inst.gamescene.m_battleFSM.IsGameState())
        {
            int min = (int)GameMgr.Inst.ginfo.StageTime / 60;
            int sec = (int)GameMgr.Inst.ginfo.StageTime % 60;
            m_txtTime.text = string.Format("Time:{0:00}:{1:00}", min, sec);
        }
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
