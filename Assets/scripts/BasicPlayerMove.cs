using UnityEngine;

public class BasicPlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // ÒÆ¶¯ËÙ¶È
    public KeyCode attackKey = KeyCode.J;
    private Rigidbody2D rb2D;
    private Animator anim;
    private Vector2 movementInput;
    private Vector2 lastMoveDir = Vector2.down;
    private bool isMoving;
    private bool isAttacking;


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>();
        UpdateAnimDir(lastMoveDir);

    }
    private void Update()
    {
        if (Input.GetKeyDown(attackKey) && !isAttacking){
            Attack();
        }
    }

    void FixedUpdate()
    {

        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        movementInput = movementInput.normalized;
        anim.SetFloat("Horizontal",movementInput.x);
        anim.SetFloat("Vertical", movementInput.y);
        anim.SetFloat("MoveSpeed", movementInput.magnitude);

        rb2D.velocity = movementInput * moveSpeed;

        bool isMoving = movementInput.magnitude > 0;
        anim.SetBool("IsMoving", isMoving);
        if(isMoving)
        {
            lastMoveDir = movementInput;
            UpdateAnimDir(lastMoveDir);
            //anim.SetFloat("Horizontal", lastMoveDir.x);
            //anim.SetFloat("Vertical", lastMoveDir.y);
        }
    }
    private void LateUpdate()
    {
        if (!isMoving)
        {
            UpdateAnimDir(lastMoveDir);
        }
    }
    private void Attack()
    {
        isAttacking = true;
        anim.SetBool("IsAttacking",true);
        Invoke("ResetAttackState", 1.0f);
    }
    private void ResetAttackState()
    {
        isAttacking = false;
        anim.SetBool("IsAttacking", false);
    }
    private void UpdateAnimDir(Vector2 dir)
    {
        anim.SetFloat("Horizontal", lastMoveDir.x);
        anim.SetFloat("Vertical", lastMoveDir.y);
        anim.Update(0);

    }

}
