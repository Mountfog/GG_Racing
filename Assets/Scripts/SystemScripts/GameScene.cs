using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GameUI m_gameUI = null;
    public HudUI m_hudUI = null;
    [HideInInspector]public BattleFSM m_battleFSM = new BattleFSM();
    public List<AudioClip> m_audioClips = new List<AudioClip>();
    public void Awake()
    {
        GameMgr.Inst.Initialize();
        GameMgr.Inst.gamescene = this;    
    }
    public void Start()
    {
        GameMgr.Inst.ginfo.Initialize();
        m_hudUI.Initialize();
        m_battleFSM.Initialize(CB_Ready, CB_Wave, CB_Game, CB_Result);
        m_battleFSM.SetReadyState();
    }
    public void CB_Ready()
    {
        m_hudUI.SetReadyState();
        m_gameUI.SetReadyState();
        GetComponent<AudioSource>().clip = m_audioClips[GameMgr.Inst.ginfo.CurStage - 1];
        GetComponent<AudioSource>().Play();
    }
    public void CB_Wave()
    {

    }
    public void CB_Game()
    {
        m_gameUI.SetGameState();
        m_hudUI.SetGameState();
    }
    public void CB_Result()
    {
        m_hudUI.SetResultState();
    }
    private void Update()
    {
        if(m_battleFSM != null)
        {
            m_battleFSM.OnUpdate();
            if (m_battleFSM.IsGameState())
                GameMgr.Inst.OnUpdate(Time.deltaTime);
        }
    }
}
