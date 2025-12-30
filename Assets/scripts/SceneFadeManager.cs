using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager Instance;

    [Header("Fade 配置")]
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        // 单例 + 跨场景
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 初始透明
        SetAlpha(0f);
    }

    private void SetAlpha(float a)
    {
        Color c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }

    /// <summary>
    /// 对外调用：淡出 → 切场景
    /// </summary>
    public void FadeAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        // 1. Fade Out
        yield return StartCoroutine(Fade(0f, 1f));

        // 2. 切场景
        SceneManager.LoadScene(sceneName);

        // 等一帧，确保新场景加载
        yield return null;

        // 3. Fade In
        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / fadeDuration);
            SetAlpha(a);
            yield return null;
        }
        SetAlpha(to);
    }
}
