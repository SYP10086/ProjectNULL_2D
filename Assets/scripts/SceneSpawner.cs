using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 场景出生点执行者（唯一负责传送玩家）
/// </summary>
public class SceneSpawner : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SetPlayerSpawnAfterLoad());
    }

    private IEnumerator SetPlayerSpawnAfterLoad()
    {
        // 等一帧，确保场景完全初始化
        yield return null;

        if (PlayerSceneData.Instance == null)
        {
            Debug.LogError("[SceneSpawner] 找不到 PlayerSceneData");
            yield break;
        }

        Vector3 spawnPos = PlayerSceneData.Instance.targetSpawnPos;

        if (spawnPos != Vector3.zero)
        {
            // 2D 游戏，z 固定为 0
            Vector3 finalPos = new Vector3(spawnPos.x, spawnPos.y, 0f);
            PlayerSceneData.Instance.transform.position = finalPos;
            Debug.Log($"[SceneSpawner] 实际设置的 Transform 是：{PlayerSceneData.Instance.gameObject.name}");

            Debug.Log($"[SceneSpawner] 玩家已传送到出生点：{finalPos}");

            // 只在这里清一次
            PlayerSceneData.Instance.ResetSpawnPoint();
        }
        else
        {
            Debug.Log("[SceneSpawner] 未指定出生点，保持原位置");
        }
    }
}
