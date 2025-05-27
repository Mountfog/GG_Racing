using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public Player m_player = null;
    public StageMgr m_stageMgr = null;
    public ItemMgr m_itemMgr = null;
    public Transform lastCameraPos = null;
    [System.Serializable]
    public struct StructSFX
    {
        public string name;
        public AudioClip clip;
    }
    public List<StructSFX> audioClipsSFX = new List<StructSFX>();
    public void PlaySFX(string name)
    {
        AudioClip kclip = null;
        foreach (var c in audioClipsSFX)
        {
            if (c.name == name)
            {
                kclip = c.clip;
                break;
            }
        }
        if (kclip == null)
            return;
        GetComponent<AudioSource>().PlayOneShot(kclip);
    }
    public void SetReadyState()
    {
        m_stageMgr.SetReadyState();
        m_player.SetReadyState();

    }
    public void SetGameState()
    {
        m_player.SetGameState();
        m_stageMgr.SetGameState();
    }
    public void SetResultState()
    {
        m_stageMgr.SetResultState();
    }

}
