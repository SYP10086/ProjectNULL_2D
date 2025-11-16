using UnityEngine;

public class BasicPlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // 移动速度
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // 需给物体添加Rigidbody2D组件
    }

    void FixedUpdate()
    {
        // 读取ADWS输入（W=上，S=下，A=左，D=右）
        float horizontal = Input.GetAxis("Horizontal"); // A=-1，D=1
        float vertical = Input.GetAxis("Vertical"); // S=-1，W=1

        // 计算移动方向（归一化避免斜向移动过快）
        Vector2 moveDir = new Vector2(horizontal, vertical).normalized;
        // 应用移动力
        rb.velocity = moveDir * moveSpeed;
    }
}
