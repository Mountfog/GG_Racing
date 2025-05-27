using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameInfo
{
    float booster = 0f;
    int curitem = -1;
    int curRank = 4;
    int curLap = 1;
    int score = 0;
    int curStage = 1;
    float stageTime = 0f;
    float curTime = 0f;
    int drift = 0;
    int itemUsed = 0;
    public bool IsGameEnded { get; set; }
    public bool IsRetire { get; set; }
    public float Booster { get {  return booster; } }
    public int CurItemId { get { return curitem; } }
    public int CurRank { get {  return curRank; } }
    public int Score { get { return score; } }
    public int CurStage { get { return curStage; } }
    public float StageTime { get { return stageTime; } }
    public int CurLap { get { return curLap; } }
    public void BoosterCharge(float value)
    {
        booster += value;
        if(booster > 100f)
        { 
            booster = 100f; 
        }
        GameMgr.Inst.gamescene.m_hudUI.m_boosterDlg.BoosterUpdate();
    }
    public void BoosterUse(float value)
    {
        if(stageTime >= 1f)
            booster -= value;
        GameMgr.Inst.gamescene.m_hudUI.m_boosterDlg.BoosterUpdate();
    }
    public bool IsBoosterUseable()
    {
        return booster >= 30 || stageTime < 0.5f;
    }
    public bool IfStartBooster()
    {
        return stageTime < 0.5f;
    }
    public bool IsBoosterMax()
    {
        return booster == 100;
    }
    public bool IsPlayerEnded()
    {
        return curLap == 4;
    }
    public void ReturnedToStart()
    {
        curLap++;
        GameMgr.Inst.gamescene.m_hudUI.m_playerRankDlg.LapUpdate();
        if (curLap == 4 && !IsRetire)
        {
            GameMgr.Inst.gamescene.m_battleFSM.SetResultState();
        }
    }
    public void RankUpdate(int krank)
    {
        curRank = krank;
        GameMgr.Inst.gamescene.m_hudUI.m_playerRankDlg.RankUpdate();
    }
    public void OnUpdate(float fElapsedTime)
    {
        stageTime += fElapsedTime;
        curTime += fElapsedTime;
        if(curTime >= 0.1f)
        {
            BoosterCharge(0.2f);
            curTime = 0f;
        }
    }
    public void Drifted()
    {
        drift++;
    }
    public void ItemUsed()
    {
        curitem = -1;
        itemUsed++;
        GameMgr.Inst.gamescene.m_hudUI.m_itemDlg.ItemUpdate();
    }
    public void ItemGet(int item)
    {
        curitem = item;
        GameMgr.Inst.gamescene.m_hudUI.m_itemDlg.ItemUpdate();
    }
    public void Initialize()
    {
        SetStage(1);
    }
    public void SetScore()
    {
        score += (drift > 20) ? 10000 :  drift * 500;
        score += (stageTime < 100) ? 10000 : (int)stageTime * 100;
        score += (itemUsed > 5) ? 10000 : itemUsed * 2000;
    }
    public void SetStage(int value)
    {
        IsGameEnded = false;
        IsRetire = false;
        drift = 0;
        booster = 0;
        itemUsed = 0;
        curitem = -1;
        curStage = value;
        curRank = value + 2;
        score = 0;
        stageTime = 0f;
        curTime = 0f;
        curLap = 1;
    }
}



