using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Rigidbody2D rb;
    bool haveGravity=true;
    float speed=10;
    float jumpSpeed = 10;
    public LayerMask groundMask;
    RaycastHit2D hit;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hit=new RaycastHit2D();
    }

    // Update is called once per frame
    void Update()
    {
        SetGravity();
        MoveWithOutGravity();
        MoveWithGravity();
    }
    void SetGravity()
    {
        if (Input.GetKeyDown(KeyCode.RightShift) && !haveGravity)
            haveGravity = true;
        else if (Input.GetKeyDown(KeyCode.RightShift) && haveGravity)
            haveGravity = false;
        
        if(Input.GetKeyDown(KeyCode.RightShift))
            rb.gravityScale = haveGravity?1:0;

    }
    void MoveWithOutGravity()
    {
        if(haveGravity) return;
        Debug.Log(rb.velocity);
        rb.velocity =new Vector2(speed * Input.GetAxis("Horizontal"), speed * Input.GetAxis("Vertical"));
    }
    void MoveWithGravity()
    {
        if (!haveGravity) return;
        rb.velocity = new Vector2(speed * Input.GetAxis("Horizontal"), rb.velocity.y);
        //Jump
        hit=Physics2D.Raycast(transform.position, new Vector2(0,-1),1f, groundMask);
        if (hit)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity= new Vector2(rb.velocity.x, rb.velocity.y+jumpSpeed);
            }
        }

    }
}
