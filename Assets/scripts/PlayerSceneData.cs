using UnityEngine;

/// <summary>
/// 玩家跨场景数据管理（只存数据，不负责传送）
/// 挂在玩家身上
/// </summary>
public class PlayerSceneData : MonoBehaviour
{
    public static PlayerSceneData Instance { get; private set; }

    [Header("跨场景出生点数据")]
    public Vector3 targetSpawnPos = Vector3.zero;
    public Quaternion targetSpawnRot = Quaternion.identity;

    [Header("示例：玩家状态")]
    public int playerHealth = 100;
    public int playerScore = 0;

    private void Awake()
    {
        // 单例
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("[PlayerSceneData] 玩家已设置为跨场景保留");
    }

    /// <summary>
    /// 清空出生点（只允许 SceneSpawner 调用）
    /// </summary>
    public void ResetSpawnPoint()
    {
        targetSpawnPos = Vector3.zero;
        targetSpawnRot = Quaternion.identity;
    }
}
