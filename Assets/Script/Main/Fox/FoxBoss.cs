using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;

using UnityEngine;

public class FoxBoss : MonoBehaviour
{
    double speed = 10, speedlocal=10;
    void SpeedClear()
    {
        speed = 0;
    }
    void SpeedRe()
    {
        speed = speedlocal;
    }
    Animator animator;
    public float chargeDistance = 4;
    public double health=100, nowHealth;
    GameObject Player, mounster;
    GameObject[] fireBall=new GameObject[8];
    new string name;
    Rigidbody2D rb;
    RaycastHit2D hit;
    double time=0,wave=2.5;
    double WaitTime = 0;
    Vector3 pos,fireBallPos;
    bool StartCharge=false, haveAttack = false, canAttack=false;
    SpriteRenderer spriteRenderer;
    bool death=false;
    //nowAnimation;//0–Ó¡¶,1≥Â
    void Hit(string a, float b)
    {
        if (a == name)
        {
            Debug.Log(a + $"Heath-{b}");
            nowHealth -= b;
        }
    }
    FoxBoss() { Event.Attack += new MyStrFloat(Hit); Event.CleanSpeed += new MyDel(SpeedClear);
        Event.SpeedRe += new MyDel(SpeedRe);
    }
    ~FoxBoss() { Event.Attack -= new MyStrFloat(Hit); Event.CleanSpeed -= new MyDel(SpeedClear);
        Event.SpeedRe -= new MyDel(SpeedRe);
    }
    double MoveCompute(double a,double max)
    {
        a = a / max;
        a = a * 2*Math.PI;
        return Math.Sin(a);
    }
    void Start()
    {
        animator=GetComponent<Animator>();
        spriteRenderer=GetComponent<SpriteRenderer>();
        for (int i=0;i<8;i++)
        {
            fireBall[i] = GameObject.Find($"FireBall/FireBall ({i+1})");
            Debug.Log(fireBall[i]);
            fireBall[i].SetActive(false);
        }
        nowHealth = health;
        Player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        mounster = this.gameObject;
        name = mounster.name;
    }
    void Update()
    {
        Debug.Log(nowHealth);
        if (nowHealth <= 0) death = true;
        Death();
        time += Time.deltaTime;
        Stratage();
        rb.velocity = Vector3.zero;
    }
    private void LateUpdate()
    {
        MoveStratage();
        time = time > 20 ?0:time;
    }
    void Attack()//’ŸªΩª«Ú
    {
        if (StartCharge) return;
        fireBallPos =Player.transform.position;
        WaitTime += Time.deltaTime;
        if(WaitTime>=1)
        {
            foreach (GameObject a in fireBall)
            {
                if(!a.activeSelf)
                {
                    a.SetActive(true);
                    FireBall fb=a.GetComponent<FireBall>();
                    fb.pos = fireBallPos;
                    break;
                }
            }
            WaitTime = 0;
        }
    }
    void Stratage()
    {
        if(death)return;
        if (speed == 0) return;
        if (time <= 8)
        {
            Attack();
        }
        else
        {
            
        }
    }
    void MoveStratage()
    {
        if (death) return;
        if (speed == 0) return;
        if (time <= 8)
        {
            MoveUp();
        }
        else
        {
            MoveToAttack();
            Charge();
        }
    }
    void MoveUp()
    {
        if (StartCharge) return;
        animator.SetInteger("nowAnimation", 0);
        Vector3 a = new Vector3(Player.transform.position.x + 6 * (float)MoveCompute(time, wave), Player.transform.position.y + 3+  (float)MoveCompute(time, wave/2), 0);
        spriteRenderer.flipX = (MoveCompute(time+wave/4, wave) > 0 ?true:false);
        if (Vector3.Distance(transform.position, a)<0.1)
        {
            transform.position = a;
            //StartFire= true;
        }
        else
            transform.position = Vector3.MoveTowards(rb.transform.position, a, 30 * Time.deltaTime);
    }
    void MoveToAttack()
    {
        if (StartCharge) return;
        //StartFire = false;
        double xd=Player.transform.position.x-transform.position.x;
        Vector3 a = new Vector3(Player.transform.position.x + (xd>0?-chargeDistance : chargeDistance), Player.transform.position.y, 0);
        transform.position = Vector3.MoveTowards(rb.transform.position, a, 20 * Time.deltaTime);
        if (Vector3.Distance(transform.position, a) < 0.1)
        {
            animator.SetInteger("nowAnimation", 1);
            WaitTime += Time.deltaTime;
            if (WaitTime>3)
            {
                WaitTime = 0;
                StartCharge = true;
                haveAttack = false;
                canAttack = true;
                animator.SetInteger("nowAnimation", 2);
                pos = new Vector3(Player.transform.position.x+ (xd > 0 ? chargeDistance : -chargeDistance), Player.transform.position.y, 0);
            }
        }
    }
    void Charge()
    {
        if (!StartCharge) return;
        if(Vector3.Distance(transform.position, pos) > 0.1)
            transform.position = Vector3.MoveTowards(rb.transform.position, pos, 10 * Time.deltaTime);
        else
        {
            animator.SetInteger("nowAnimation", 0);
            canAttack = false;
            WaitTime += Time.deltaTime;
            if(WaitTime > 2)
            {
               StartCharge = false;
               WaitTime = 0;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (haveAttack||!canAttack||collision.gameObject.name!="Player") return;
        Debug.Log("hit");
        float damage = 20f + UnityEngine.Random.value * 3 * (UnityEngine.Random.value >= 0.5 ? 1 : -1);
        Event.Hit(damage);
        haveAttack = true;
    }
    void Death()
    {
        
        if (!death) return;
        Debug.Log("Death");
        BossEnd bossEnd = GameObject.Find("ControlDeliver").GetComponent<BossEnd>();
        bossEnd.yes = true;
    }
}
