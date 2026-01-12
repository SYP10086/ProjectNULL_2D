using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 2D 场景切换触发器
/// 挂在传送门 / 门口 / 区域触发器上
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class SceneSwitchTrigger : MonoBehaviour
{
    [Header("目标场景")]
    public string targetSceneName;

    [Header("目标场景出生点")]
    public Vector3 targetSpawnPosition;

    [Header("触发角色 Tag")]
    public string playerTag = "Player";

    private void Awake()
    {
        // 自动设置为 Trigger
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (PlayerSceneData.Instance == null)
        {
            Debug.LogError("[SceneSwitchTrigger] 玩家未挂 PlayerSceneData");
            return;
        }

        // 1️⃣ 存出生点
        PlayerSceneData.Instance.targetSpawnPos = targetSpawnPosition;

        Debug.Log($"[SceneSwitchTrigger] 存储出生点：{targetSpawnPosition}");

        // 2️⃣ 切场景（只一次）
        Debug.Log(SceneFadeManager.Instance);
        SceneFadeManager.Instance.FadeAndLoadScene(targetSceneName);
    }

#if UNITY_EDITOR
    // Scene 视图辅助显示
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetSpawnPosition, 0.3f);
    }
#endif
}