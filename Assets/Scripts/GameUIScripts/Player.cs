using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 15f;
    [SerializeField] float brake = 15f;
    [SerializeField] float boosterSpeed = 15f;
    [SerializeField] GameObject[] wheelModels = new GameObject[4];
    [SerializeField] List<WheelCollider> wheels = new List<WheelCollider>();
    Rigidbody rb = null;
    [SerializeField] ParticleSystem m_boosterEffect = null;
    [SerializeField] ParticleSystem m_shieldEffect = null;
    [SerializeField] ParticleSystem m_bombEffect = null;
    [SerializeField] List<Transform> startingPoints = new List<Transform>();
    [SerializeField] float radius = 6;
    [SerializeField] float driftStiffness = 0.4f;
    [SerializeField] float normalStiffness = 1f;
    [SerializeField] float downwardsForceValue = 1f;
    public GameObject m_trails = null;
    bool IsImmortal = true;
    WheelFrictionCurve flwheelFrictionCurve;
    WheelFrictionCurve frwheelFrictionCurve;
    WheelFrictionCurve slwheelFrictionCurve;
    WheelFrictionCurve srwheelFrictionCurve;

    public float curSpeed = 0f;
    bool IsBoosterUsing = false;
    float driftTime = 0f;
    public int distance = 0;
    List<GameObject> cubes = new List<GameObject>();
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1, 0);
        for(int i = 0; i < 4; i++)
        {
            wheels[i].transform.position = wheelModels[i].transform.position;
        }
        flwheelFrictionCurve = wheels[2].forwardFriction;
        frwheelFrictionCurve = wheels[3].forwardFriction;
        slwheelFrictionCurve = wheels[2].sidewaysFriction;
        srwheelFrictionCurve = wheels[3].sidewaysFriction;
        cubes = GameMgr.Inst.gamescene.m_gameUI.m_stageMgr.m_distanceCubes;
    }
    public void SetReadyState()
    {
        Invoke(nameof(Initialize),0.01f);
        Invoke(nameof(Initialize), 0.02f);
        GetComponentInChildren<FollowPlayer>().SetReadyState();
    }
    public void Initialize()
    {
        IsImmortal = false;
        IsBoosterUsing = false;
        rb.velocity = Vector3.zero;
        for (int i = 0; i < wheels.Count; i++)
        {
            wheels[i].motorTorque = 0;
            wheels[i].steerAngle = 0;
            wheels[i].rotationSpeed = 0;
        }
        m_trails.SetActive(false);
        rb.position = startingPoints[GameMgr.Inst.ginfo.CurStage + 1].position;
        m_boosterEffect.gameObject.SetActive(false);
        m_shieldEffect.gameObject.SetActive(false);
        m_bombEffect.gameObject.SetActive(false);
        transform.eulerAngles = new Vector3(0, -90, 0);
    }
    public void SetGameState()
    {
        m_trails.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveInput();
        WheelMeshUpdate();
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
    void MoveInput()
    {
        if (GameMgr.Inst.gamescene.m_battleFSM.IsGameState())
        {
            ForwardUpdate();
            SteerUpdate();
            DriftUpdate();
            BrakeUpdate();
            ItemUpdate();
            BoosterUpdate();
            SpeedUpdate();
            AddDownWardsForce();
        }
    }
    void ForwardUpdate()
    {
        for (int i = 0; i < wheels.Count; i++)
        {
            wheels[i].motorTorque = Input.GetAxis("Vertical") * speed;
        }
    }
    void SteerUpdate()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * MathF.Atan(2.55f/(radius + (1.5f / 2f))) * Input.GetAxis("Horizontal"); 
            wheels[1].steerAngle = Mathf.Rad2Deg * MathF.Atan(2.55f / (radius - (1.5f / 2f))) * Input.GetAxis("Horizontal");
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * MathF.Atan(2.55f / (radius - (1.5f / 2f))) * Input.GetAxis("Horizontal");
            wheels[1].steerAngle = Mathf.Rad2Deg * MathF.Atan(2.55f / (radius + (1.5f / 2f))) * Input.GetAxis("Horizontal");
        }
        else
        {
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }
    }
    void BoosterUpdate()
    {
        if(Input.GetAxis("Booster") > 0 && !IsBoosterUsing)
        {
            if (!GameMgr.Inst.ginfo.IsBoosterUseable())
            {
                return;
            }
            if (!GameMgr.Inst.ginfo.IsBoosterMax())
            {
                GameMgr.Inst.ginfo.BoosterUse(30);
                if (GameMgr.Inst.ginfo.IfStartBooster())
                {
                    StartCoroutine(BoosterCoolDown(1f));
                }
                else
                {
                    StartCoroutine(BoosterCoolDown(0.5f));
                }
            }
            else
            {
                GameMgr.Inst.ginfo.BoosterUse(100);
                StartCoroutine(BoosterCoolDown(2f));
            }
            gameObject.GetComponentInChildren<FollowPlayer>().BoosterUsed();

        }
    }
    IEnumerator BoosterCoolDown(float fTime)
    {
        IsBoosterUsing = true;
        m_boosterEffect.gameObject.SetActive(true);
        m_boosterEffect.Play();
        GameMgr.Inst.gamescene.m_gameUI.PlaySFX("Booster");
        float kspeed = 100;
        speed = boosterSpeed;
        if (curSpeed < 40)
        {
            speed *= 3;
        }
        yield return new WaitForSeconds(fTime);
        speed = kspeed;
        IsBoosterUsing = false;
        m_boosterEffect.Stop();
        yield return new WaitForSeconds(4f);
        m_boosterEffect.gameObject.SetActive(false);
        yield return null;
    }
    void BrakeUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GameMgr.Inst.gamescene.m_gameUI.PlaySFX("Brake");
        }
        for (int i = 0; i < 4; i++)
        {
            wheels[i].brakeTorque = Input.GetAxis("Brake") * brake;
        }
    }
    void DriftUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            flwheelFrictionCurve.stiffness = driftStiffness;
            wheels[2].forwardFriction = flwheelFrictionCurve;
            frwheelFrictionCurve.stiffness = driftStiffness;
            wheels[3].sidewaysFriction = frwheelFrictionCurve;
            slwheelFrictionCurve.stiffness = driftStiffness;
            wheels[2].forwardFriction = slwheelFrictionCurve;
            srwheelFrictionCurve.stiffness = driftStiffness;
            wheels[3].sidewaysFriction = srwheelFrictionCurve;
            driftTime += Time.deltaTime;
            if (driftTime >= 0.1f)
            {
                GameMgr.Inst.ginfo.BoosterCharge(0.4f);
                driftTime = 0f;
            }
        }
        else
        {
            flwheelFrictionCurve.stiffness = normalStiffness;
            wheels[2].forwardFriction = flwheelFrictionCurve;
            frwheelFrictionCurve.stiffness = normalStiffness;
            wheels[3].sidewaysFriction = frwheelFrictionCurve;
            slwheelFrictionCurve.stiffness = normalStiffness;
            wheels[2].forwardFriction = slwheelFrictionCurve;
            srwheelFrictionCurve.stiffness = normalStiffness;
            wheels[3].sidewaysFriction = srwheelFrictionCurve;
        }
    }
    void SpeedUpdate()
    {
        curSpeed = rb.velocity.magnitude;
        GameMgr.Inst.gamescene.m_hudUI.m_speedDlg.SpeedUpdate();
    }
    void ItemUpdate()
    {
        if(Input.GetAxis("Item") > 0)
        {
            if (GameMgr.Inst.ginfo.CurItemId == -1)
            {
                return;
            }
            if (GameMgr.Inst.ginfo.CurItemId == 0)
            {
                StartCoroutine(BoosterCoolDown(0.5f));
                gameObject.GetComponentInChildren<FollowPlayer>().BoosterUsed();
            }
            if (GameMgr.Inst.ginfo.CurItemId == 1)
            {
                StartCoroutine(BoosterCoolDown(1f));
                gameObject.GetComponentInChildren<FollowPlayer>().BoosterUsed();
            }
            if (GameMgr.Inst.ginfo.CurItemId == 2)
            {
                GameMgr.Inst.gamescene.m_gameUI.m_stageMgr.BombUsed();
            }
            if(GameMgr.Inst.ginfo.CurItemId == 3)
            {
                StartCoroutine(ShieldCor());
            }
            GameMgr.Inst.ginfo.ItemUsed();
        }
    }
    void AddDownWardsForce()
    {
        rb.AddForce(-transform.up * downwardsForceValue * rb.velocity.magnitude);
    }
    void WheelMeshUpdate()
    {
        for(int i = 0; i < 4; i++)
        {
            Vector3 wheelPos = Vector3.zero;
            Quaternion wheelRotation = Quaternion.identity;

            wheels[i].GetWorldPose(out wheelPos, out wheelRotation);
            wheelModels[i].transform.SetPositionAndRotation(wheelPos, wheelRotation); 
        }
    }
    private void Update()
    {
        CheatCodes();
    }
    public void CheatCodes()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameMgr.Inst.ginfo.SetStage(1);
            GameMgr.Inst.gamescene.m_battleFSM.SetReadyState();
        }
        if(Input.GetKeyDown(KeyCode.F2))
        {
            GameMgr.Inst.ginfo.SetStage(2);
            GameMgr.Inst.gamescene.m_battleFSM.SetReadyState();
        }
        if(Input.GetKeyDown(KeyCode.F3))
        {
            GameMgr.Inst.ginfo.SetStage(3);
            GameMgr.Inst.gamescene.m_battleFSM.SetReadyState();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            int item = UnityEngine.Random.Range(0, 4);
            GameMgr.Inst.ginfo.ItemGet(item);
        }
        if(Input.GetKeyDown(KeyCode.F5))
        {
            //속도 강화
        }
        if(Input.GetKeyDown(KeyCode.F6))
        {
            IsImmortal = true;
        }
        if(Input.GetKeyDown(KeyCode.F7))
        {
            IsImmortal = false;
        }
        if(Input.GetKeyDown(KeyCode.F8))
        {
            GameMgr.Inst.gamescene.m_gameUI.m_stageMgr.BombAll();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            GameMgr.Inst.ginfo.BoosterCharge(100);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LapEnd"))
        {
            if (GameMgr.Inst.ginfo.StageTime > 10)
            {
                GameMgr.Inst.ginfo.ReturnedToStart();
                GameMgr.Inst.gamescene.m_hudUI.m_playerRankDlg.LapAlarm();
                GameMgr.Inst.gamescene.m_gameUI.PlaySFX("LapEnd");
            }
        }
        if (other.CompareTag("LastScene"))
        {
            if (GameMgr.Inst.ginfo.CurLap == 3)
            {
                GetComponentInChildren<FollowPlayer>().LastScene();
            }
        }
        if (other.CompareTag("ItemGetto"))
        {
            GameMgr.Inst.gamescene.m_gameUI.PlaySFX("LapEnd");
            int item = UnityEngine.Random.Range(0, 4);
            GameMgr.Inst.ginfo.ItemGet(item);
        }
    }
    public void Bomb()
    {
        StartCoroutine(BombCheck());
    }
    IEnumerator BombCheck()
    {
        yield return new WaitForSeconds(2f);
        if (!IsImmortal)
        {
            StartCoroutine(BombCor());
        }
        else
        {
            GameMgr.Inst.gamescene.m_hudUI.m_itemDlg.BombBlocked();
        }
        yield return null;
    }
    IEnumerator BombCor()
    {
        m_bombEffect.gameObject.SetActive(true);
        m_bombEffect.Play();
        GameMgr.Inst.gamescene.m_gameUI.PlaySFX("Bomb");
        for (int i = 0; i < wheels.Count; i++)
        {
            wheels[i].rotationSpeed = 0;
            wheels[i].motorTorque = 0;
        }
        rb.velocity /= 3;
        float kspeed = 100;
        IsBoosterUsing = true;
        speed = 0;
        yield return new WaitForSeconds(1f);
        IsBoosterUsing = false;
        speed = kspeed;
        yield return new WaitForSeconds(2f);
        m_bombEffect.gameObject.SetActive(false);
        yield return null;
    }
    IEnumerator ShieldCor()
    {
        m_shieldEffect.gameObject.SetActive(true);
        m_shieldEffect.Play();
        IsImmortal = true;
        yield return new WaitForSeconds(2.5f);
        IsImmortal = false;
        yield return new WaitForSeconds(0.5f);
        m_shieldEffect.gameObject.SetActive(false);
        yield return null;
    }
}
