using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class HandsCntrol : MonoBehaviour
{
    public float speed = 1;
    float speedlocal = 1;
    bool haveAttack;
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
        Attack();
        AttackOver();
    }
    void Attack()
    {
        if (!monster1.attack) return;
        if(monster1.attackTime<=0.5f)
        transform.localPosition += new Vector3(speed * Time.deltaTime,0 ,0);
        else
            transform.localPosition -= new Vector3(speed * Time.deltaTime,0 , 0);
    }
    void AttackOver()
    {
        if (monster1.attack) return;
        transform.localPosition = new Vector3((-hand.transform.localScale.x+monster.transform.localScale.x)/2, 0, 0);
        haveAttack = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (haveAttack || collision == null) return;
        
        float damage =10f+ Random.value*3*(Random.value>=0.5?1:-1);
        Event.Hit(damage);
        haveAttack =true;
    }
}
