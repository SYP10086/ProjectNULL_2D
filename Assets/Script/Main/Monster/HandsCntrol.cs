using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class HandsCntrol : MonoBehaviour
{
    public float speed = 0.32f;
    float speedlocal = 0.32F;
    public bool haveAttack;
    GameObject monster, hand;
    Monster monster1;
    // Update is called once per frame
    void SpeedClear()
    {
        speed = 0;
    }
    void SpeedRe()
    {
        speed = speedlocal;
    }
    ~HandsCntrol()
    {
        Event.CleanSpeed -= new MyDel(SpeedClear);
        Event.SpeedRe -= new MyDel(SpeedRe);
    }
    HandsCntrol()
    {
        Event.CleanSpeed += new MyDel(SpeedClear);
        Event.SpeedRe += new MyDel(SpeedRe);
    }
    private void Start()
    {
        Transform parentTransform = transform.parent;
        monster = GameObject.Find($"{parentTransform.name}");
        monster1 = monster.GetComponent<Monster>();
        hand = this.gameObject;
        speed=speedlocal;
    }
    void LateUpdate()
    {
        if(monster1.tower=='R') speed =Mathf.Abs(speed);
        else speed = -Mathf.Abs(speed);
            Attack();
        AttackOver();
    }
    void Attack()
    {
        if (!monster1.attack) return;
        if(monster1.attackTime<=0.65f)
        transform.localPosition += new Vector3(speed * Time.deltaTime,0 ,0);
        else
            transform.localPosition -= new Vector3(speed * Time.deltaTime,0 , 0);
    }
    void AttackOver()
    {
        if (monster1.attack) return;
        transform.localPosition = new Vector3(0, 0, 0);
        haveAttack = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (haveAttack || collision == null|| !monster1.attack) return;
        
        float damage =10f+ Random.value*3*(Random.value>=0.5?1:-1);
        Event.Hit(damage);
        haveAttack =true;
    }
}
