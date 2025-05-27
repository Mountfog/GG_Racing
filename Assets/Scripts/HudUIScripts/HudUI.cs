using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : MonoBehaviour
{
    public SpeedDlg m_speedDlg = null;
    public ItemDlg m_itemDlg = null;
    public PlayerRankDlg m_playerRankDlg = null;
    public BoosterDlg m_boosterDlg = null;
    public ScoreDlg m_scoreDlg = null;

    public ResultDlg m_resultDlg = null;
    public GameRankDlg m_gameRankDlg = null;
    public OptionsDlg m_optionsDlg = null;
    public PausePanelDlg m_pausePanelDlg = null;

    public Text m_txtReady = null;
    public Text m_txtRetire = null;
    public void Initialize()
    {
        m_resultDlg.Initialize();
        m_pausePanelDlg.Initialize();
    }
    public void SetReadyState()
    {
        HideUI(m_txtRetire.gameObject);
        m_boosterDlg.SetReadyState();
        m_speedDlg.SetReadyState();
        m_playerRankDlg.SetReadyState();
        m_scoreDlg.SetReadyState();
        m_resultDlg.SetReadyState();
        StartCoroutine(ReadyCor());
        m_itemDlg.SetReadyState();
    }
    public void SetGameState()
    {
        HideUI(m_txtRetire.gameObject);
        m_boosterDlg.SetGameState();
        m_speedDlg.SetGameState();
        m_playerRankDlg.SetGameState();
    }
    public void SetResultState()
    {
        HideUI(m_txtRetire.gameObject);
        m_boosterDlg.SetResultState();
        m_speedDlg.SetResultState();
        m_playerRankDlg.SetResultState();
        m_scoreDlg.SetResultState();
        StartCoroutine(ResultCor());
        m_itemDlg.SetResultState();
    }
    IEnumerator ReadyCor()
    {
        m_txtReady.color = Color.red;
        m_txtReady.text = "3";
        ActivateUI(m_txtReady.gameObject);
        yield return new WaitForSeconds(1f);
        m_txtReady.color = Color.yellow;
        m_txtReady.text = "2";
        yield return new WaitForSeconds(1f);
        m_txtReady.color = Color.green;
        m_txtReady.text = "1";
        yield return new WaitForSeconds(1f);
        m_txtReady.text = "Start!!";
        StartCoroutine(RainbowReady());
        GameMgr.Inst.gamescene.m_battleFSM.SetGameState();
        yield return null;
    }
    IEnumerator RainbowReady()
    {
        float curTime = 0f;
        float lerpTime = 1f;
        while(curTime != lerpTime)
        {
            curTime += Time.deltaTime;
            if (curTime > lerpTime)
                curTime = lerpTime;
            List<float> rand = new List<float>();
            for (int i = 0; i < 3; i++)
            {
                float krand = Random.Range(0, 2);
                rand.Add(krand);
            }
            Color kcolor = new Color(rand[0], rand[1], rand[2]);
            m_txtReady.color = kcolor;
            yield return null;
        }
        HideUI(m_txtReady.gameObject);
        yield return null;
    }
    public void HideUI(GameObject go)
    {
        go.SetActive(false);
    }
    public void ActivateUI(GameObject go)
    {
        go.SetActive(true);
    }
    public void RetireCountStart()
    {
        GameMgr.Inst.ginfo.IsGameEnded = true;
        StartCoroutine(RetireCountStartCor());
    }
    IEnumerator RetireCountStartCor()
    {
        float curtime = 0f;
        float lerpTime = 11f;
        ActivateUI(m_txtRetire.gameObject);
        while(curtime != lerpTime)
        {
            curtime += Time.deltaTime;
            if(curtime > lerpTime)
                curtime = lerpTime;
            if(GameMgr.Inst.ginfo.CurLap == 4)
            {
                break;
            }
            int sec = (int)curtime;
            m_txtRetire.text = (sec < 10) ? (10 - sec).ToString() : "GameOver";
            if(sec >= 10)
            {
                GameMgr.Inst.ginfo.IsRetire = true;
            }
            yield return null;
        }
        GameMgr.Inst.gamescene.m_battleFSM.SetResultState();
        yield return null;
    }
    IEnumerator ResultCor()
    {
        yield return new WaitForSeconds(1f);
        m_resultDlg.SetResultState();
        yield return null;
    }
    private void Update()
    {
        if (GameMgr.Inst.gamescene.m_battleFSM.IsGameState())
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_pausePanelDlg.PressedESC();
            }
        }
    }
}
