using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerMain : MonoBehaviour
{
    public static bool haveGravity = false;//控制有无重力,地面laywer为Wall
    public static float healthy = 100;
    public static float nowHealthy = 100;
    public static float stamina = 100;
    public static float nowStamina = 100;
    public static float attackDamage = 20, attackTime, attackLimit = 1f;
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
    public static float speed = 4;
    public static float speedthis = speed;
    public static bool death, attack, canShifit = true;
    public static Vector3 clickPoint;
    GameObject Hand;
    BasicPlayerMove basicPlayerMove;
    float waitDebug;
    public float g = 1,j=1;
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
    public void HealthLose(float damage)
    {
        nowHealthy -= damage;
    }
    void SpeedClear()
    {
        Time.timeScale = 0;
        speedthis =( speed==0)? speedthis:speed;
        speed = 0;
    }
    void SpeedRe()
    {
        Time.timeScale = 1;
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
        basicPlayerMove=GetComponent<BasicPlayerMove>();
        transform =GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        Hand = GameObject.Find("Player/Hand");
        speed = speedthis;
    }
    
    void Update()
    {
        scene= SceneManager.GetActiveScene();
        if (scene.name!= "FoxBattle"&& scene.name != "TreeBattle")
            haveGravity=false;
        else haveGravity=true;
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
        rb.gravityScale = haveGravity ? g : 0;
        Move();
        MoveWithGravity();
    }
    void MoveWithGravity()
    {
        if (attack) return;
        if (!haveGravity) return;
        float xv = Input.GetAxis("Horizontal")*2;
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) xv = 0;
        if (nowStamina < 0)
        {
            canShifit = false;
        }
        if (Input.GetKey(KeyCode.LeftShift) && nowStamina > 0 && canShifit)
        {
            rb.velocity = new Vector2(xv * speed * shiftPower, rb.velocity.y);
            nowStamina -= Time.deltaTime * staminaDecrease;
            ReTime = 0;
        }
        else if (!canShifit)
        {
            rb.velocity = new Vector2(xv * speed * shiftLose, rb.velocity.y);
            if (nowStamina < stamina)
                nowStamina += Time.deltaTime * staminaEDIncrease;
            else
            {
                canShifit = true;
                stamina = nowStamina;
            }
        }
        else if (xv == 0)
        {
            if (ReTime <= 100)
                ReTime += Time.deltaTime;
            if (ReTime >= ReWait * 2)
            {
                if (nowStamina < stamina)
                    nowStamina += Time.deltaTime * staminaIncrease * 2;
                else
                {
                    stamina = nowStamina;
                }
            }
            else if (ReTime >= ReWait)
            {
                if (nowStamina < stamina)
                    nowStamina += Time.deltaTime * staminaIncrease * 1.1f;
                else
                {
                    stamina = nowStamina;
                }
            }
        }
        else
        {
            rb.velocity = new Vector2(xv * speed, rb.velocity.y);
            if (ReTime <= 100)
                ReTime += Time.deltaTime;
            if (ReTime >= ReWait)
            {
                ReTime = ReWait;
                if (nowStamina < stamina)
                    nowStamina += Time.deltaTime * staminaIncrease * 0.9f;
                else
                {
                    stamina = nowStamina;
                }
            }
        }
        //Jump
        //Collider2D collider=GetComponent<Collider2D>();
        //RaycastHit2D hit1 = Physics2D.Raycast(collider.transform.position+new Vector3(collider.bounds.size.x/2,0,0), new Vector2(0, -1), collider.bounds.size.y / 2 + j, LayerMask.GetMask("Wall") + LayerMask.GetMask("Monster"));
        //RaycastHit2D hit2 = Physics2D.Raycast(collider.transform.position - new Vector3(collider.bounds.size.x / 2, 0, 0), new Vector2(0, -1), collider.bounds.size.y/2+j,LayerMask.GetMask("Wall")+ LayerMask.GetMask("Monster"));
        //if (hit1|| hit2)
        //{
        //    if (Input.GetKey(KeyCode.Space))
        //    {
        //        rb.velocity = new Vector2(rb.velocity.x, speed*5);
        //    }
        //}

    }
    private void Move()
    {
        rb.velocity = new Vector2(0,0);
        if (attack ) return;
        if (haveGravity) return;
        
        float xv = Input.GetAxis("Horizontal"), yv = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) xv = 0;
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) yv = 0;
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
        Time.timeScale = 0;
        Event.Death();
        Death.ShowDeath();
    }
    void Attack()
    {
        if (basicPlayerMove== null) return;
        if(Input.GetKeyDown(basicPlayerMove.attackKey)&&!attack&&speed!=0)
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
