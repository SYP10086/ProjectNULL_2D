using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FireBall : MonoBehaviour
{
    new string name;
    double speed = 4,startDis=6;
    public Vector3 pos,start;
    bool init=false;
    private void Start()
    {
        name = this.gameObject.name;
        transform.position =new Vector3(0,10,0);
    }
    public void Fire()
    {
        if(Vector3.Distance(transform.position, pos) < 0.01f)
        {
            init = false;
            this.gameObject.SetActive(false);
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, pos, (float)speed * Time.deltaTime);
    }
    void Update()
    {
        Init();
    }
    private void LateUpdate()
    {
        Fire();
    }
    void Init()
    {
        if (init) return;
        double a = UnityEngine.Random.value;
        a = a * Math.PI / 2;
        a=a-Math.PI / 4;
            transform.position = pos+new Vector3((float)(startDis*Math.Sin(a)), (float)(startDis * Math.Cos(a)), 0);
            transform.rotation = Quaternion.LookRotation(pos - transform.position);
            Vector2 v = pos - transform.position;
            var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            var trailRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            transform.rotation = trailRotation;
        init = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name=="Player")
        {
            float damage = 10f + UnityEngine.Random.value * 3 * (UnityEngine.Random.value >= 0.5 ? 1 : -1);
            Event.Hit(damage);
            init = false;
            this.gameObject.SetActive(false);
        }
        else
        {
            init = false;
            this.gameObject.SetActive(false);
        }
    }
}
