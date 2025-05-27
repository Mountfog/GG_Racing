using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ResultDlg : MonoBehaviour
{
    public Transform m_panel = null;
    public Button m_btnNextStage = null;
    public Button m_btnMainMenu = null;
    public Button m_btnRestart = null;
    public Text m_txtTitle = null;
    public Text m_txtScore = null;
    public Text m_TxtRank = null;
    public Text m_txtTime = null;


    public Transform m_startPos = null;
    public Transform m_endPos = null;

    // Start is called before the first frame update
    public void Initialize()
    {
        m_btnNextStage.onClick.AddListener(OnClick_NextStage);
        m_btnMainMenu.onClick.AddListener(OnClick_MainMenu);
        m_btnRestart.onClick.AddListener(OnClick_ReStage);
    }
    public void SetReadyState()
    {

        m_panel.gameObject.SetActive(false);
        m_panel.position = m_endPos.position;
    }
    public void SetResultState()
    {
        GameMgr.Inst.ginfo.SetScore();
        m_txtScore.text = string.Format("Score : {0:00000}",GameMgr.Inst.ginfo.Score);
        int rank = GameMgr.Inst.ginfo.CurRank;
        if (rank == 1)
        {
            m_TxtRank.text = "1st";
        }
        else if (rank == 2)
        {
            m_TxtRank.text = "2nd";
        }
        else if (rank == 3)
        {
            m_TxtRank.text = "3rd";
        }
        else
        {
            m_TxtRank.text = string.Format("{0}th", rank);
        }
        int min = (int)GameMgr.Inst.ginfo.StageTime / 60;
        int sec = (int)GameMgr.Inst.ginfo.StageTime % 60;
        m_txtTime.text = string.Format("Time: {0:00}:{1:00}", min, sec);
        if (GameMgr.Inst.ginfo.CurRank == 1)
        {
            m_txtTitle.text = "Winner";
        }
        else if (GameMgr.Inst.ginfo.IsPlayerEnded())
        {
            m_txtTitle.text = "Finished";
        }
        else
        {
            m_txtTitle.text = "Retired";
        }
        m_btnNextStage.gameObject.SetActive(!(GameMgr.Inst.ginfo.CurStage == 3));
        StartCoroutine(ResultCor());
    }
    public void OnClick_NextStage()
    {
        GameMgr.Inst.ginfo.SetStage(GameMgr.Inst.ginfo.CurStage+1);
        GameMgr.Inst.gamescene.m_battleFSM.SetReadyState();
    }
    public void OnClick_ReStage()
    {
        GameMgr.Inst.ginfo.SetStage(GameMgr.Inst.ginfo.CurStage);
        GameMgr.Inst.gamescene.m_battleFSM.SetReadyState();
    }
    IEnumerator ResultCor()
    {
        m_panel.gameObject.SetActive(true);
        float curtime = 0f;
        float lerptime = 1.5f;
        while(curtime != lerptime)
        {
            curtime += Time.deltaTime;
            if(curtime > lerptime)
                curtime  = lerptime;
            float x = curtime / lerptime;
            float t = UsedBounce(x);
            m_panel.position = Vector3.Lerp(m_startPos.position, m_endPos.position, t);
            yield return null;
        }
        yield return null;
    }
    public void OnClick_MainMenu()
    {
        SceneManager.LoadScene("TitleScene");
    }
    float UsedBounce(float x)
    {
        return 1 - EaseOutBounce(x);
    }
    float EaseOutBounce(float x)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;
        if(x < 1f / d1)
        {
            return n1 * x * x;
        }
        else if(x < 2f / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        else if(x > 2.5f / d1)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }
}
