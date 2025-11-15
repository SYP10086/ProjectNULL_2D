
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    Vector2 weaponPos;
    public float weaponDistance=1;
    GameObject weapon,player;
    public static bool inIt;
    Vector3 dir;
    double angle;
    double attackAngle=180;
    bool haveAttack;
    
    private void Start()
    {
        weapon = GameObject.Find("Player/Hand");
        player = GameObject.Find("Player");
    }
    private void LateUpdate()
    {
        Attack();
    }
    void Attack()
    {
        if (!PlayerMain.attack)
        {
            haveAttack = false;
            weapon.SetActive(false);
            return;
        }
        Vector3 weaponTower = weapon.transform.position- player.transform.position;
        float angle2= Vector3.SignedAngle(Vector3.up, weaponTower, Vector3.forward);
        transform.eulerAngles = new Vector3(0,0,angle2);
        if (!inIt)
        {
            dir = new Vector3(Camera.main.ScreenToWorldPoint(PlayerMain.clickPoint).x,Camera.main.ScreenToWorldPoint(PlayerMain.clickPoint).y,0) - player.transform.position;
            Debug.Log(dir);
            angle = Vector3.SignedAngle(Vector3.left, dir, Vector3.forward)+180;
            transform.localPosition =new Vector2(weaponDistance*(float)Math.Cos((angle+ attackAngle/2) * Mathf.Deg2Rad), weaponDistance*(float)Math.Sin((angle + attackAngle/2) * Mathf.Deg2Rad)) ;
            inIt = true;
        }
        else
        {
            float x = PlayerMain.attackTime / PlayerMain.attackLimit;
            angle -= (attackAngle * Time.deltaTime / PlayerMain.attackLimit) * (x> 0.5 ?3.25-3*x:0.25+3*x);
            transform.localPosition = new Vector2(weaponDistance*(float)System.Math.Cos((angle + attackAngle/2) * Mathf.Deg2Rad), weaponDistance*(float)Math.Sin((angle + attackAngle/2) * Mathf.Deg2Rad));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (haveAttack || collision == null) return;
        float damage = PlayerMain.attackDamage + UnityEngine.Random.value * PlayerMain.attackDamage/10 * (UnityEngine.Random.value >= 0.5 ? 1 : -1);
        Event.Attack(collision.gameObject.name, damage);
        haveAttack = true;
    }
}
