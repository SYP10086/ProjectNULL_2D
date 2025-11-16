using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 拖入角色
    public SpriteRenderer background; // 拖入背景Sprite对象
    public Vector3 offset = new(0, 0, -10);
    public float smoothSpeed = 5f;
    private Bounds backgroundBounds;
    private float camHalfWidth;
    private float camHalfHeight;

    void Start()
    {
        Camera mainCam = GetComponent<Camera>();
        camHalfHeight = mainCam.orthographicSize;
        camHalfWidth = camHalfHeight * mainCam.aspect;
        backgroundBounds = background.bounds;
    }

    void LateUpdate()
    {
        if (target == null || background == null) return;

        // 目标位置=角色位置+偏移，限制在背景内
        Vector3 targetPos = target.position + offset;
        targetPos.x = Mathf.Clamp(targetPos.x, backgroundBounds.min.x + camHalfWidth, backgroundBounds.max.x - camHalfWidth);
        targetPos.y = Mathf.Clamp(targetPos.y, backgroundBounds.min.y + camHalfHeight, backgroundBounds.max.y - camHalfHeight);

        // 平滑移动
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
    }
}
