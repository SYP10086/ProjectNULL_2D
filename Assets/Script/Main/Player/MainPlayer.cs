using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerMain : MonoBehaviour
{

    public static float healthy = 100;
    public static float nowHealthy = 100;
    public static float stamina = 100;
    public static float nowStamina = 100;
    public static float attackDamage = 20, attackTime, attackLimit = 0.4f;
    public static float backAttack = 2;
    public float shiftPower = 1.5f, shiftLose = 0.7f;
    public float staminaIncrease = 20, staminaDecrease = 20, staminaEDIncrease = 15, ReWait = 1;
    public static new Transform transform;
    public static float x, y, ReTime = 0;
    Scene scene;
    //Save
    public static bool initLocation = false;
    //
    static Rigidbody2D rb;
    public static float speed = 10;
    public static float speedthis = 4;
    public static bool death, attack, canShifit = true;
    public static Vector3 clickPoint;
    GameObject Hand;
    float waitDebug;
    void ItemUse()
    {
        string A=null;
        if (Input.GetKeyDown(KeyCode.H))
            A = "Health";
        if (A != null)
            Event.ItemUse(A);
    }
    void DebugWithTime(GameObject a)
    {
        if(waitDebug>1)
        {
            Debug.Log(a);
            waitDebug=0;
        }
        else
        {
            waitDebug += Time.deltaTime;
        }
    }
    public static void StartPlayer()//初始化血量与耐力
    {
        healthy = 100;
        nowHealthy = healthy;
        stamina = 100;
        nowStamina = stamina;
        attackDamage = 20;
        death = false;
        speed = speedthis;
        canShifit = true;
    }
    void HealthLose(float damage)
    {
        nowHealthy -= damage;
    }
    void SpeedClear()
    {
        speed = 0;
    }
    void SpeedRe()
    {
        speed = speedthis;
    }
    ~PlayerMain()
    {
        Event.CleanSpeed -= new MyDel(SpeedClear);
        Event.SpeedRe -= new MyDel(SpeedRe);
        Event.Hit -= new Myfloat(HealthLose);
    }
    PlayerMain()
    {
        Event.CleanSpeed += new MyDel(SpeedClear);
        Event.SpeedRe += new MyDel(SpeedRe);
        Event.Hit += new Myfloat(HealthLose);
    }
    void Start()
    {
        transform =GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        Hand = GameObject.Find("Player/Hand");
        speed = speedthis;
    }
    void Update()
    {
        
        if (!initLocation&&(x!=0||y!=0))
        {
            transform.position=new Vector2 (x,y);
            x=0; y = 0;
            initLocation = true;
            attack = false;
        }
        ChangeSence();
        DeathDetect();
        Attack();
        ItemUse();
    }
    private void LateUpdate()
    {
        Move();
    }
    private void Move()
    {
        float xv = Input.GetAxis("Horizontal"), yv = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(xv, 0, yv);
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }
        xv=movement.x; 
        yv=movement.z;
        if (nowStamina < 0)
        {
            canShifit = false;
        }
        if (Input.GetKey(KeyCode.LeftShift)&&nowStamina>0&& canShifit)
        {
            rb.velocity = new Vector2(xv * speed*shiftPower, yv * speed* shiftPower);
            nowStamina -= Time.deltaTime * staminaDecrease;
            ReTime = 0;
        }
        else if(!canShifit)
        {
            rb.velocity = new Vector2(xv * speed* shiftLose, yv * speed* shiftLose);
            if(nowStamina< stamina)
            nowStamina+= Time.deltaTime * staminaEDIncrease;
            else
            {
                canShifit = true;
                stamina = nowStamina;
            }
        }
        else if(xv == 0&& yv == 0)
        {
            if (ReTime <= 100)
                ReTime += Time.deltaTime;
            if (ReTime >= ReWait*2)
            {
                if (nowStamina < stamina)
                    nowStamina += Time.deltaTime * staminaIncrease*2;
                else
                {
                    stamina = nowStamina;
                }
            }
            else if (ReTime >= ReWait)
            {
                if (nowStamina < stamina)
                    nowStamina += Time.deltaTime * staminaIncrease*1.1f;
                else
                {
                    stamina = nowStamina;
                }
            }
        }
        else
        {
            rb.velocity = new Vector2(xv * speed, yv * speed);
            if (ReTime <= 100)
                ReTime += Time.deltaTime;
            if (ReTime >= ReWait)
            {
                ReTime = ReWait;
                if (nowStamina < stamina)
                    nowStamina += Time.deltaTime * staminaIncrease*0.9f;
                else
                {
                    stamina = nowStamina;
                }
            }
        }
    }
    void ChangeSence()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex+1);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartPlayer();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            nowHealthy -= 10;
        }
    }
    void DeathDetect()
    {
        if (nowHealthy > 0)
        {  return; }
        Event.CleanSpeed();
        death = true;
        Event.Death();
        Death.ShowDeath();
    }
    void Attack()
    {
        if(Input.GetMouseButtonDown(0)&&!attack&&speed!=0)
        { 
            clickPoint =Input.mousePosition;
            attack = true;
            Weapon.inIt = false;
            if(Hand!=null)
            Hand.SetActive(true);
        }
        if (attack) 
        { 
            attackTime += Time.deltaTime;
            if (attackTime >= attackLimit)
            {
                attack = false;
                attackTime = 0;
            }
        }
    }
}
