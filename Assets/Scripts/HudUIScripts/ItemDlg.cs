using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDlg : MonoBehaviour
{

    public GameObject m_itemDlg = null;
    public Text m_txtItem = null;
    public Text m_block = null;
    public Text m_exclaim = null;

    public void SetReadyState()
    {
        m_txtItem.text = "";
        ActivateUI(m_itemDlg);
        HideUI(m_block.gameObject);
        HideUI(m_exclaim.gameObject);
    }
    public void SetResultState()
    {
        HideUI(m_itemDlg);
        HideUI(m_block.gameObject);
    }
    public void ItemUpdate()
    {
        int curItemId = GameMgr.Inst.ginfo.CurItemId;
        if (curItemId == -1)
        {
            m_txtItem.text = "";
        }
        if(curItemId == 0)
        {
            m_txtItem.text = "Free Booster";
        }
        if (curItemId == 1)
        {
            m_txtItem.text = "Max Booster";
        }
        if (curItemId == 2)
        {
            m_txtItem.text = "Bomb";
        }
        if (curItemId == 3)
        {
            m_txtItem.text = "Shield";
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
    public void BombBlocked()
    {
        ActivateUI(m_block.gameObject);
        Invoke(nameof(HideBlock),1);
    }
    public void HideBlock()
    {
        HideUI(m_block.gameObject);
    }
    public void Bombgo()
    {
        ActivateUI(m_exclaim.gameObject);
        Invoke(nameof(Gobomb), 1f);
    }
    public void Gobomb()
    {
        HideUI(m_exclaim.gameObject);
    }
}
