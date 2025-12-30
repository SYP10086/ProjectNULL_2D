using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 2D 相机跟随（跨场景唯一 Player 版本）
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraFollow2D_FIXED : MonoBehaviour
{
    public Vector3 followOffset = new Vector3(0, 1, -10);
    public float smoothSpeed = 6f;

    [Header("背景限制")]
    public string backgroundTag = "Background";

    private Camera cam;
    private Transform playerTrans;

    // 背景边界
    private SpriteRenderer bgRenderer;
    private Vector2 bgSize;
    private Vector2 bgCenter;
    private float left, right, bottom, top;

    private static CameraFollow2D_FIXED instance;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (instance != null && instance != this)
        {
            Destroy(gameObject);   // 销毁新生成的 Camera
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        BindPlayer();
        RefreshBgBounds();
    }

    private void LateUpdate()
    {
        if (playerTrans == null) return;
        Follow();
    }

    private void BindPlayer()
    {
        if (PlayerSceneData.Instance != null)
        {
            playerTrans = PlayerSceneData.Instance.transform;
            Debug.Log("[Camera] 已绑定 PlayerSceneData.Instance");
        }
        else
        {
            Debug.LogWarning("[Camera] PlayerSceneData.Instance 尚未存在");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindPlayer();
        RefreshBgBounds();
    }

    private void Follow()
    {
        Vector3 target = playerTrans.position + followOffset;
        Vector3 smooth = Vector3.Lerp(transform.position, target, smoothSpeed * Time.deltaTime);

        Vector3 final = new Vector3(
            Mathf.Clamp(smooth.x, left, right),
            Mathf.Clamp(smooth.y, bottom, top),
            followOffset.z
        );

        transform.position = final;
    }

    private void RefreshBgBounds()
    {
        GameObject bg = GameObject.FindWithTag(backgroundTag);
        if (bg == null) return;

        bgRenderer = bg.GetComponent<SpriteRenderer>();
        if (bgRenderer == null) return;

        bgSize = bgRenderer.bounds.size;
        bgCenter = bg.transform.position;

        float camHalfH = cam.orthographicSize;
        float camHalfW = camHalfH * Screen.width / Screen.height;

        left = bgCenter.x - bgSize.x / 2 + camHalfW;
        right = bgCenter.x + bgSize.x / 2 - camHalfW;
        bottom = bgCenter.y - bgSize.y / 2 + camHalfH;
        top = bgCenter.y + bgSize.y / 2 - camHalfH;
    }
}
