using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject detect;
    GameObject Player, mounster;
    Rigidbody2D rb;
    RaycastHit2D hit;
    RaycastHit2D[] WallHit;
    public LayerMask Wall;
    new string name;
    Vector3 PlayerLastLocation;
    bool HaveHit = false;
    public bool attack=false;
    public float speed = 2, x, y, turnSpeed = 180;
    public float attackTime = 0;
    public float dectectLong = 4;
    public float detectAngle = 120;
    public float health1 = 50;
    float health = 1;
    bool StartMove = false;
    void SpeedClear()
    {
        speed = 0;
    }
    void SpeedRe()
    {
        speed = 2;
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
    Monster()
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
        WallHit =new RaycastHit2D[8];
        detect = GameObject.Find($"{this.gameObject.name}/Detect");
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
        Debug.DrawLine(rb.transform.position, Player.transform.position, Color.red);
        IfFindPlayer();
        SetTheScaleOfDetect();
        Attack();
    }
    private void LateUpdate()
    {
        MoveTowerPlayer();
        AwayWall();
        Death();
    }
    public void Init()
    {
        Debug.Log(name + " Init");
        HaveHit = false;
        StartMove = true;
        attack = false;
        health = health1;
        rb.transform.position = new Vector2(x, y);
        PlayerLastLocation = rb.transform.position + Vector3.up;
    }
    public void Death()
    {
        if (health >= 0)
        {
            return;
        }
        else
        {
            Debug.Log(mounster+ "SetActive(false)");
            health =health1;
            mounster.SetActive(false);
        }
    }
    void Attack()
    {
        Vector3 dir = Player.transform.position - transform.position;
        float angle = (Vector3.SignedAngle(Vector3.left, dir, Vector3.forward) + 180);
        angle = Math.Abs(angle - transform.eulerAngles.z) > 360 - Math.Abs(angle - transform.eulerAngles.z) ? ((angle - transform.eulerAngles.z > 0) ? angle - transform.eulerAngles.z - 360 : angle - transform.eulerAngles.z + 360) : angle - transform.eulerAngles.z;
        if ((angle > -1 && angle < 1 && Vector3.Distance(rb.transform.position, Player.transform.position) < 1.7f)|| attack)
        {
            attack = true;
            attackTime += Time.deltaTime;
            if (attackTime >=1)
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
        rb.velocity = Vector3.zero;
        rb.angularVelocity= 0;
        if (!StartMove) return;
        if (attack) return;
        if (!HaveHit)
        {
            Vector3 dir = Player.transform.position - transform.position;
            float angle = (Vector3.SignedAngle(Vector3.left, dir, Vector3.forward) + 180);
            angle = Math.Abs(angle - transform.eulerAngles.z) > 360 - Math.Abs(angle - transform.eulerAngles.z) ? ((angle - transform.eulerAngles.z > 0) ? angle - transform.eulerAngles.z - 360 : angle - transform.eulerAngles.z + 360) : angle - transform.eulerAngles.z;
            if (angle >= 1)
                transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
            else if (angle <= -1)
                transform.Rotate(0, 0, -turnSpeed * Time.deltaTime);
            rb.transform.position = Vector3.MoveTowards(rb.transform.position, Player.transform.position, speed * Time.deltaTime);
        }
        else if (HaveHit)
        {
            Vector3 dir = PlayerLastLocation - transform.position;
            float angle = (Vector3.SignedAngle(Vector3.left, dir, Vector3.forward) + 180);
            angle = Math.Abs(angle - transform.eulerAngles.z) > 360 - Math.Abs(angle - transform.eulerAngles.z) ? ((angle - transform.eulerAngles.z > 0) ? angle - transform.eulerAngles.z - 360 : angle - transform.eulerAngles.z + 360) : angle - transform.eulerAngles.z;
            if (angle >= 1)
                transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
            else if (angle <= -1)
                transform.Rotate(0, 0, -turnSpeed * Time.deltaTime);
            rb.transform.position = Vector3.MoveTowards(rb.transform.position, PlayerLastLocation, speed * Time.deltaTime);
            if (Vector3.Distance(rb.transform.position, PlayerLastLocation) < 0.1f && angle>-1&& angle<1)
            {
                HaveHit = false;
                StartMove = false;
            }
        }
    }
    void IfFindPlayer()
    {
        
        hit = Physics2D.Raycast(rb.transform.position, Player.transform.position- rb.transform.position, Mathf.Infinity, Wall +LayerMask.GetMask("Player"));

        if (hit) 
        {
            float angle = Vector3.Angle(Player.transform.position-transform.position , transform.right);
            if (hit.collider.name=="Player"&& hit.distance< dectectLong&& angle < detectAngle/2)
            {
                PlayerLastLocation = hit.point;
                HaveHit = false;
                StartMove =true;
            }
            else
            {
                HaveHit = true;
            }
        } 
    }
}
