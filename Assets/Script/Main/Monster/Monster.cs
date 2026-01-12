using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.XR;

public class Monster : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    public GameObject detect, hand;
    GameObject Player, mounster,foot,playerFoot;
    Rigidbody2D rb;
    RaycastHit2D hit;
    RaycastHit2D[] WallHit;
    public LayerMask Wall;
    new string name;
    Vector3 PlayerLastLocation;
    bool HaveHit = false;
    public bool attack=false;
    public float speed = 1, x, y, turnSpeed = 180;
    public float attackTime = 0;
    public float dectectLong = 4;
    public float detectAngle = 270;
    public float health1 = 50;
    float health = 1;
    bool StartMove = false;
    public char tower='R';
    double time=0;
    bool death = false;
    void SpeedClear()
    {
        speed = 0;
    }
    void SpeedRe()
    {
        speed = 1;
    }
    void Hit(string a,float b)
    {
        if(a == name)
        {
            Debug.Log(a + $"Heath-{b}");
            if(HaveHit)
            { 
                health -= b*PlayerMain.backAttack;
                Player = GameObject.Find("Player");
                PlayerLastLocation = Player.transform.position;
                StartMove = true;
            }
            else
            {
                health -= b;
            }
        }
    }
    public Monster()
    {
        Event.CleanSpeed += new MyDel(SpeedClear);
        Event.SpeedRe += new MyDel(SpeedRe);
        Event.Attack += new MyStrFloat(Hit);
        Event.LocalChange += new MyDel(Init);
    }
    ~Monster()
    {
        Event.CleanSpeed -= new MyDel(SpeedClear);
        Event.SpeedRe -= new MyDel(SpeedRe);
        Event.Attack -= new MyStrFloat(Hit);
        Event.LocalChange -= new MyDel(Init);
    }
    void Start()
    {
        playerFoot= GameObject.Find($"Player/Foot");
        foot = GameObject.Find($"AllMonster/{this.gameObject.name}/FOOT");
        animator =GetComponent<Animator>();
        spriteRenderer =GetComponent<SpriteRenderer>();
        WallHit =new RaycastHit2D[8];
        detect = GameObject.Find($"{this.gameObject.name}/Detect");
        hand = GameObject.Find($"{this.gameObject.name}/Hand");
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player");
        Wall = LayerMask.GetMask("Wall");
        x=transform.position.x;
        y=transform.position.y;
        mounster = this.gameObject;
        name = mounster.name;
        health = health1;
    }
    void Update()
    {
        Debug.DrawRay(transform.position, transform.right, Color.red);
        Debug.DrawLine(rb.transform.position, Player.transform.position, Color.red);
        IfFindPlayer();
        SetTheScaleOfDetect();
        Attack();
    }
    private void LateUpdate()
    {
        UpdateAnimation();
        MoveTowerPlayer();
        //AwayWall();
        Death();
    }
    public void Init()
    {
        time = 0;
        Debug.Log(name + " Init");
        HaveHit = false;
        StartMove = true;
        attack = false;
        health = health1;
        if(transform!=null)
        transform.position = new Vector2(x, y);
    }
    public void Death()
    {
        if (health >= 0)
        {
            return;
        }
        else
        {
            death = true;
            hand.SetActive(false);
            attack = true;
            time += Time.deltaTime;
            if(time>=2)
            {
                time = 0;
                health =health1;
                mounster.SetActive(false);
                Debug.Log(mounster+ "SetActive(false)");
            }
            
        }
    }
    void Attack()
    {
        if (death) return;
        double xd = playerFoot.transform.position.x - foot.transform.position.x;
        Vector3 a = new Vector3(playerFoot.transform.position.x + (xd > 0 ? -0.9F : 0.9F), playerFoot.transform.position.y + 0.11f, 0);
        if (( Vector3.Distance(foot.transform.position, a) < 0.1f)|| attack)
        {
            attack = true;
            attackTime += Time.deltaTime;
            if (attackTime >=1.4f)
            {
                attack = false;
                attackTime = 0;
            }
        }
    }
    void AwayWall()
    {
        double destanceOfWall;
        for (int i = -1; i < 2; i++)
        { 
            for(int j = -1; j<2;j++ )
            {
                if (j == 0 && i == 0)
                    continue;
                if (i == 1 && j == 1)
                {
                    WallHit[4] = Physics2D.Raycast(rb.transform.position, new Vector2(i, j), Mathf.Infinity, Wall);
                }
                else WallHit[(i + 1) * 3 + j + 1] = Physics2D.Raycast(rb.transform.position, new Vector2(i, j), Mathf.Infinity, Wall);
            }
        }
        for(int i=0;i<8;i++)
        {
            if (WallHit[i])
            {
                destanceOfWall=WallHit[i].distance;
                if(destanceOfWall<1)
                rb.transform.position = Vector2.MoveTowards(rb.transform.position, WallHit[i].point-new Vector2(rb.transform.position.x, rb.transform.position.y), -(float)(1/destanceOfWall)* Time.deltaTime);
            }
            else
                continue;
        }
    }
    void SetTheScaleOfDetect()
    {
        detect.transform.localScale = new Vector3(dectectLong * 2, dectectLong * 2,dectectLong * 2);
    }
    void MoveTowerPlayer()//////
    {
        if (death) return;
        rb.velocity = Vector3.zero;
        rb.angularVelocity= 0;
        if (!StartMove) return;
        if (attack) return;
        if (!HaveHit)
        {
            Vector3 dir = playerFoot.transform.position - foot.transform.position;
            double xd = playerFoot.transform.position.x - foot.transform.position.x;
            double dis = Vector3.Distance(foot.transform.position, transform.position);
            Vector3 a = new Vector3(playerFoot.transform.position.x + (xd > 0 ? -0.9F : 0.9F), playerFoot.transform.position.y+(float)dis+0.11f, 0);
            rb.transform.position = Vector3.MoveTowards(rb.transform.position, a, speed * Time.deltaTime);
        }
        //else if (HaveHit)
        //{
        //    double dis = Vector3.Distance(foot.transform.position, transform.position);
        //    rb.transform.position = Vector3.MoveTowards(rb.transform.position, PlayerLastLocation+new Vector3(0, (float)dis+0.11f, 0), speed * Time.deltaTime);
        //    if (Vector3.Distance(rb.transform.position, PlayerLastLocation) < 0.1f)
        //    {
        //        HaveHit = false;
        //        StartMove = false;
        //    }
        //}
    }
    void IfFindPlayer()
    {
        if(death)return;
        hit = Physics2D.Raycast(foot.transform.position, playerFoot.transform.position- foot.transform.position, dectectLong, Wall+LayerMask.GetMask("Player"));

        if (hit) 
        {
            float angle = Vector3.Angle(playerFoot.transform.position- foot.transform.position , transform.right);
            
            if (hit.collider.name=="Player"&& hit.distance< dectectLong)
            {
                if (angle >= 90) 
                {
                    tower = 'L';
                }
                else
                {
                    tower = 'R';
                }
                double xd = playerFoot.transform.position.x - foot.transform.position.x;
                Vector3 a = new Vector3(playerFoot.transform.position.x + (xd > 0 ? -0.9F : 0.9F), playerFoot.transform.position.y, 0);
                PlayerLastLocation = a;
                HaveHit = false;
                StartMove =true;
            }
            else
            {
                HaveHit = true;
            }
        } 
    }
    void UpdateAnimation()
    {
        if (health > 0) { 
        if (tower == 'L') spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
        }
        animator.SetBool("Range", StartMove);
        animator.SetBool("PlayerInAttackRange", attack);
        animator.SetFloat("Healthy", health);
        animator.SetBool("Direction", UnityEngine.Random.value>0.5);
    }
}
