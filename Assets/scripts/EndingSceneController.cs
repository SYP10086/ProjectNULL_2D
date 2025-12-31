using UnityEngine;

public class EndingSceneController : MonoBehaviour
{
    private void Start()
    {
        if (PlayerSceneData.Instance != null)
        {
            PlayerSceneData.Instance.gameObject.SetActive(false);
        }
    }
}
