using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowPlayer : MonoBehaviour
{
    public Transform cameraView = null;
    public Transform cameraPos = null;
    public Transform cameraBoosterPos = null;
    public float speed = 5f;
    public bool isPlayerFollow = true;
    public bool isLastScene = false;

    Vector3 oldPos = Vector3.zero;
    float oldSpeed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        cameraBoosterPos.localPosition = Vector3.Lerp(cameraView.localPosition, cameraPos.localPosition, 0.7f);
        oldPos = cameraPos.localPosition;
        oldSpeed = speed;
    }
    public void SetReadyState()
    {
        isPlayerFollow = true;
        isLastScene = false;
        gameObject.transform.localPosition = cameraPos.localPosition;
        gameObject.transform.localEulerAngles = cameraPos.localEulerAngles;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isPlayerFollow)
        {
            Vector3 pos = Vector3.Lerp(transform.position, cameraPos.position, Time.deltaTime * speed);
            transform.position = pos;
            transform.LookAt(cameraView);
        }
        if (isLastScene)
        {
            transform.position = GameMgr.Inst.gamescene.m_gameUI.lastCameraPos.position;
            transform.eulerAngles = GameMgr.Inst.gamescene.m_gameUI.lastCameraPos.eulerAngles;
        }
    }
    public void BoosterUsed()
    {
        if(!isLastScene)
            StartCoroutine(BoosterCamera());
    }
    IEnumerator BoosterCamera()
    {
        cameraPos.localPosition = cameraBoosterPos.localPosition;
        speed = speed * 100;
        yield return new WaitForSeconds(0.1f);
        cameraPos.localPosition = oldPos;
        speed = oldSpeed;
        yield return null;
    }
    public void LastScene()
    {
        isPlayerFollow = false;
        isLastScene = true;
    }
    
}
