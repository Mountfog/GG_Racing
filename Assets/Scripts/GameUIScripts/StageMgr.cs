using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StageMgr : MonoBehaviour
{
    public List<GameObject> m_enemies = new List<GameObject>();
    public List<CEnemy> m_curEnemies = new List<CEnemy>();

    public List<GameObject> m_distanceCubes = new List<GameObject>();

    // Start is called before the first frame update
    public void SetReadyState()
    {
        m_curEnemies.Clear();
        foreach(var obj in m_enemies)
        {
            obj.SetActive(false);
        }
        for(int i=0;i<GameMgr.Inst.ginfo.CurStage + 1; i++)
        {
            m_enemies[i].SetActive(true);
            CEnemy kenemy = m_enemies[i].GetComponent<CEnemy>();
            m_curEnemies.Add(kenemy);
            kenemy.SetReadyState();
        }
    }
    public void SetGameState()
    {
        foreach (var obj in m_curEnemies)
        {
            obj.SetGameState();
        }
    }
    public void SetResultState()
    {
        foreach(var obj in m_curEnemies)
        {
            obj.SetResultState();
        }
    }

    public void FixedUpdate()
    {
        if (GameMgr.Inst.gamescene.m_battleFSM.IsGameState())
        {
            int playerLap = GameMgr.Inst.ginfo.CurLap;
            int playerDistance = GameMgr.Inst.gamescene.m_gameUI.m_player.distance;
            int rank = 1;
            foreach(var obj in m_curEnemies)
            {
                if(playerLap < obj.lap)
                {
                    rank++;
                }
                else if(playerDistance < obj.distance && playerLap == obj.lap)
                {
                    rank++;
                }
            }
            GameMgr.Inst.ginfo.RankUpdate(rank);
        }
    }
    public void BombUsed()
    {
        BombCheck();
    }
    public void BombCheck()
    {
        if (GameMgr.Inst.ginfo.CurRank == 1)
        {
            GameMgr.Inst.gamescene.m_hudUI.m_itemDlg.Bombgo();
            GameMgr.Inst.gamescene.m_gameUI.m_player.Bomb();
        }
        else
        {
            CEnemy target = null;
            foreach(var obj in m_curEnemies)
            {
                if (target == null)
                    target = obj;
                else
                {
                    if (target.lap < obj.lap)
                    {
                        target = obj;
                    }
                    else if(target.distance < obj.distance)
                    {
                        target = obj;
                    }
                }
            }
            target.Bomb();
        }
    }
    public void BombAll()
    {
        foreach (var obj in m_curEnemies)
        {
            obj.Bomb();
        }
    }
}
