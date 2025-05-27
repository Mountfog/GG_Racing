using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr
{
    static GameMgr instance = null;
    public static GameMgr Inst
    {
        get
        {
            if (instance == null)
                instance = new GameMgr();
            return instance;
        }
    }
    public void Initialize()
    {
        IsInstalled = true;
        Application.runInBackground = true;
    }
    public void OnUpdate(float fTime)
    {
        ginfo.OnUpdate(fTime);
    }
    public bool IsInstalled { get; set; }
    public GameInfo ginfo = new GameInfo();
    public GameScene gamescene { get; set; }


}
