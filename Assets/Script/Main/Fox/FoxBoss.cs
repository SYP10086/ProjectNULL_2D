using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class FoxBoss : MonoBehaviour
{
    GameObject Player;
    Rigidbody2D rb;
    RaycastHit2D hit;
    public float dectectLong = 4;
    public float detectAngle = 120;
    void Start()
    {
        Player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        
    }
    void IfFindPlayer()
    {

        hit = Physics2D.Raycast(rb.transform.position, Player.transform.position - rb.transform.position, Mathf.Infinity, LayerMask.GetMask("Wall") + LayerMask.GetMask("Player"));

        if (hit)
        {
            float angle = Vector3.Angle(Player.transform.position - transform.position, transform.right);
            if (hit.collider.name == "Player" && hit.distance < dectectLong && angle < detectAngle / 2)
            {

            }
            else
            {
                
            }
        }
    }
}
