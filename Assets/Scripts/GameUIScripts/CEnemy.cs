using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemy : MonoBehaviour
{

    [SerializeField]Vector3 startPos = Vector3.zero;
    [SerializeField] float enemySpeed = 20f;
    [SerializeField] float rotationSpeed = 9f;
    bool isCorner = false;
    public int lap = 0;
    public int distance = 0;
    public float downwardsForceValue = 100f;
    List<GameObject> cubes = new List<GameObject>();
    public List<GameObject> wheels = new List<GameObject>();
    [SerializeField] ParticleSystem m_shieldEffect = null;
    [SerializeField] ParticleSystem m_bombEffect = null;
    public GameObject trail = null;
    bool isMove = true;
    public int item = -1;
    // Start is called before the first frame update
    void Start()
    {
        cubes = GameMgr.Inst.gamescene.m_gameUI.m_stageMgr.m_distanceCubes;
    }
    public void Initialize()
    {
        trail.SetActive(false);
        isMove = false;
        lap = 1;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_shieldEffect.gameObject.SetActive(false);
        m_bombEffect.gameObject.SetActive(false);
        transform.eulerAngles = new Vector3(0, -90, 0);
        transform.position = startPos;
        isCorner = false;
    }
    public void SetReadyState()
    {
        Invoke(nameof(Initialize), 0.01f);
        Invoke(nameof(Initialize), 0.02f);
    }
    public void SetGameState()
    {
        trail.SetActive(true);
        isMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        EnemyMove();
        DistanceCheck();
    }
    void DistanceCheck()
    {
        if (GameMgr.Inst.gamescene.m_battleFSM.IsGameState())
        {
            float minDist = 100f;
            for (int i = 0; i < cubes.Count; i++)
            {
                float dist = Vector3.Distance(transform.position, cubes[i].transform.position);
                if (minDist > dist)
                {
                    minDist = dist;
                    distance = i;
                }
            }
        }
    }
    public void SetResultState()
    {
        Invoke(nameof(EnemyStop), 5f);
    }
    void EnemyStop()
    {
        isMove = false;
    }
    void EnemyMove()
    {
        if (isMove)
        {
            Vector3 vec = transform.forward.normalized * enemySpeed * Time.deltaTime;
            vec = new Vector3(vec.x, (vec.y > 0.1f) ? 0.1f : vec.y,  vec.z);
            GetComponent<Rigidbody>().MovePosition(transform.position + vec);
            if (isCorner)
            {
                transform.eulerAngles += new Vector3(0, -rotationSpeed * Time.deltaTime, 0);
            }
            GetComponent<Rigidbody>().AddForce(-transform.up * downwardsForceValue * GetComponent<Rigidbody>().velocity.magnitude);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CornerStart"))
        {
            isCorner = true;
        }
        if (other.CompareTag("CornerEnd"))
        {
            isCorner = false;
        }
        if (other.CompareTag("LapEnd"))
        {
            if(GameMgr.Inst.ginfo.StageTime > 10)
            {
                lap++;
                if (lap == 4)
                {
                    if (!GameMgr.Inst.ginfo.IsPlayerEnded() && !GameMgr.Inst.ginfo.IsGameEnded)
                    {
                        GameMgr.Inst.gamescene.m_hudUI.RetireCountStart();
                       
                    }
                }
            }
            
        }
        if (other.CompareTag("ItemGetto"))
        {
            item = UnityEngine.Random.Range(0, 4);
            StartCoroutine(ItemUse());
        }
        
    }
    IEnumerator ItemUse()
    {
        yield return new WaitUntil(()=>isCorner == false);
        float rand = UnityEngine.Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(rand);
        if (item == 0)
        {
            GameMgr.Inst.gamescene.m_gameUI.m_stageMgr.BombUsed();
            item = -1;
        }
        yield return null;
    }
    public void Bomb()
    {
        StartCoroutine(BombCheck());
    }
    IEnumerator BombCheck()
    {
        yield return new WaitForSeconds(2f);
        if (item == 1)
        {
            StartCoroutine(ShieldCor());
            item = -1;
        }
        else
        {
            StartCoroutine(BombCor());
        }
        yield return null;
    }
    IEnumerator BombCor()
    {
        m_bombEffect.gameObject.SetActive(true);
        m_bombEffect.Play();
        isMove = false;
        yield return new WaitForSeconds(1.5f);
        isMove = true;
        yield return new WaitForSeconds(1.5f);
        m_bombEffect.gameObject.SetActive(false);
        yield return null;
    }
    IEnumerator ShieldCor()
    {
        m_shieldEffect.gameObject.SetActive(true);
        m_shieldEffect.Play();
        yield return new WaitForSeconds(2.5f);
        yield return new WaitForSeconds(0.5f);
        m_shieldEffect.gameObject.SetActive(false);
        yield return null;
    }
}
