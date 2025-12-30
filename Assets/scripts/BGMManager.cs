using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent] // 防止重复挂载
public class BGMManager : MonoBehaviour
{
    #region 单例与核心配置
    public static BGMManager Instance { get; private set; }

    [Header("基础音频配置")]
    [Tooltip("普通场景通用BGM")]
    public AudioClip normalBGM;
    [Tooltip("默认音量（0-1）")]
    [Range(0f, 1f)] public float defaultVolume = 0.8f;
    [Tooltip("最小音量（防止完全静音）")]
    [Range(0f, 0.1f)] public float minVolume = 0.01f;

    [Header("过渡效果配置")]
    [Tooltip("渐变总时长（秒），建议1.5-3秒")]
    public float fadeDuration = 2f;
    [Tooltip("过渡曲线（S形更自然）")]
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("多Boss BGM映射")]
    [Tooltip("Key=Boss场景名，Value=对应BGM")]
    public List<BossBGMMap> bossBGMMap = new List<BossBGMMap>();
    #endregion

    #region 私有变量
    private AudioSource bgmSourceA; // 主音频源（普通BGM/当前Boss BGM）
    private AudioSource bgmSourceB; // 副音频源（过渡用）
    private Dictionary<string, AudioClip> bossBGMdic; // 缓存Boss BGM字典
    private Coroutine fadeCoroutine; // 当前过渡协程（用于中断）
    private bool isFading = false; // 过渡中标记
    private AudioClip currentPlayingClip; // 当前播放的BGM
    #endregion

    // 多Boss BGM映射结构体（Inspector可视化）
    [System.Serializable]
    public struct BossBGMMap
    {
        public string bossSceneName; // Boss场景名（精确匹配）
        public AudioClip bossBGMClip; // 对应BGM
    }

    #region 生命周期
    private void Awake()
    {
        // 单例初始化（跨场景唯一）
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 初始化双AudioSource（自动创建，避免手动添加）
        InitDualAudioSources();

        // 初始化Boss BGM字典（优化查找效率）
        InitBossBGMDictionary();

        // 监听场景加载事件（核心：自动识别场景切换）
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // 初始场景自动播放普通BGM（无音乐时触发）
        if (normalBGM != null && currentPlayingClip == null)
        {
            PlayBGM(normalBGM, bgmSourceA);
            currentPlayingClip = normalBGM;
        }
    }

    private void OnDestroy()
    {
        // 移除监听，避免内存泄漏
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
    }
    #endregion

    #region 初始化方法
    /// <summary>
    /// 初始化双AudioSource（叠加过渡用）
    /// </summary>
    private void InitDualAudioSources()
    {
        // 创建/获取音频源A
        bgmSourceA = GetOrAddAudioSource("BGMSource_A");
        // 创建/获取音频源B
        bgmSourceB = GetOrAddAudioSource("BGMSource_B");

        // 基础配置
        bgmSourceA.loop = true;
        bgmSourceB.loop = true;
        bgmSourceA.volume = defaultVolume;
        bgmSourceB.volume = 0f;
    }

    /// <summary>
    /// 获取/创建指定名称的AudioSource
    /// </summary>
    private AudioSource GetOrAddAudioSource(string sourceName)
    {
        AudioSource source = transform.Find(sourceName)?.GetComponent<AudioSource>();
        if (source == null)
        {
            GameObject sourceObj = new GameObject(sourceName);
            sourceObj.transform.SetParent(transform);
            source = sourceObj.AddComponent<AudioSource>();
        }
        return source;
    }

    /// <summary>
    /// 初始化Boss BGM字典（将List转为Dictionary，提升查找速度）
    /// </summary>
    private void InitBossBGMDictionary()
    {
        bossBGMdic = new Dictionary<string, AudioClip>();
        foreach (var map in bossBGMMap)
        {
            if (!string.IsNullOrEmpty(map.bossSceneName) && map.bossBGMClip != null)
            {
                // 覆盖重复场景名（避免冲突）
                if (bossBGMdic.ContainsKey(map.bossSceneName))
                {
                    Debug.LogWarning($"重复的Boss场景名：{map.bossSceneName}，已覆盖原有BGM");
                    bossBGMdic[map.bossSceneName] = map.bossBGMClip;
                }
                else
                {
                    bossBGMdic.Add(map.bossSceneName, map.bossBGMClip);
                }
            }
            else
            {
                Debug.LogWarning($"Boss BGM映射配置错误：场景名或音频为空");
            }
        }
    }
    #endregion

    #region 核心工具方法
    /// <summary>
    /// 播放指定BGM到指定音频源（修复未定义错误的核心）
    /// </summary>
    private void PlayBGM(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        source.volume = defaultVolume;
        source.Play();
    }
    #endregion

    #region 场景加载回调（核心逻辑）
    /// <summary>
    /// 场景加载完成后自动判断是否切换BGM
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        Debug.Log($"场景加载完成：{sceneName}，开始检测BGM切换");

        // 1. 如果是Boss场景，切换到对应Boss BGM
        if (bossBGMdic.TryGetValue(sceneName, out AudioClip bossClip))
        {
            SmoothSwitchBGM(bossClip);
        }
        // 2. 如果不是Boss场景且当前播放的是Boss BGM，切回普通BGM
        else if (currentPlayingClip != normalBGM && IsBossClip(currentPlayingClip))
        {
            SmoothSwitchBGM(normalBGM);
        }
    }

    /// <summary>
    /// 判断当前音频是否是Boss BGM（用于场景切换校验）
    /// </summary>
    private bool IsBossClip(AudioClip clip)
    {
        foreach (var kvp in bossBGMdic)
        {
            if (kvp.Value == clip) return true;
        }
        return false;
    }
    #endregion

    #region 核心：平滑切换BGM（叠加淡入淡出）
    /// <summary>
    /// 对外暴露：平滑切换到指定BGM（支持手动触发）
    /// </summary>
    /// <param name="targetClip">目标BGM</param>
    public void SmoothSwitchBGM(AudioClip targetClip)
    {
        // 校验：目标音频为空/与当前播放一致 → 直接返回
        if (targetClip == null || currentPlayingClip == targetClip)
        {
            Debug.Log($"无需切换BGM：目标音频为空或与当前一致");
            return;
        }

        // 中断正在进行的过渡（避免叠加）
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            isFading = false;
        }

        // 启动新的过渡协程
        fadeCoroutine = StartCoroutine(FadeBetweenBGM(targetClip));
    }

    /// <summary>
    /// 叠加淡入淡出协程（核心：无静音间隙，超平滑过渡）
    /// </summary>
    private IEnumerator FadeBetweenBGM(AudioClip targetClip)
    {
        isFading = true;
        Debug.Log($"开始过渡到BGM：{targetClip.name}，时长：{fadeDuration}秒");

        // 确定源/目标音频源：A播放当前，B播放新的；交替使用
        AudioSource fromSource = currentPlayingClip == bgmSourceA.clip ? bgmSourceA : bgmSourceB;
        AudioSource toSource = fromSource == bgmSourceA ? bgmSourceB : bgmSourceA;

        // 初始化目标音频源
        toSource.clip = targetClip;
        toSource.Play();
        toSource.volume = minVolume; // 避免完全静音（保留一点点底音更自然）

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            // 计算插值进度（通过动画曲线实现非线性过渡）
            float t = elapsedTime / fadeDuration;
            float curveT = fadeCurve.Evaluate(t);

            // 源音频淡出，目标音频淡入（叠加）
            fromSource.volume = Mathf.Lerp(defaultVolume, minVolume, curveT);
            toSource.volume = Mathf.Lerp(minVolume, defaultVolume, curveT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 过渡完成：清理源音频
        fromSource.volume = minVolume;
        fromSource.Stop(); // 停止源音频（节省性能）
        toSource.volume = defaultVolume; // 确保目标音频音量到位

        // 更新当前播放状态
        currentPlayingClip = targetClip;
        isFading = false;
        fadeCoroutine = null;

        Debug.Log($"BGM过渡完成，当前播放：{currentPlayingClip.name}");
    }
    #endregion

    #region 辅助方法（手动控制/调试）
    /// <summary>
    /// 手动触发切换到指定Boss场景的BGM（比如同场景激活Boss）
    /// </summary>
    /// <param name="bossSceneName">Boss场景名（需在映射表中配置）</param>
    public void ManualSwitchToBossBGM(string bossSceneName)
    {
        if (bossBGMdic.TryGetValue(bossSceneName, out AudioClip clip))
        {
            SmoothSwitchBGM(clip);
        }
        else
        {
            Debug.LogError($"未找到Boss场景[{bossSceneName}]对应的BGM，请检查映射配置");
        }
    }

    /// <summary>
    /// 手动切回普通BGM（比如Boss战结束）
    /// </summary>
    public void ManualSwitchToNormalBGM()
    {
        if (normalBGM == null)
        {
            Debug.LogError("普通BGM未配置，无法切换");
            return;
        }
        SmoothSwitchBGM(normalBGM);
    }

    /// <summary>
    /// 暂停/恢复所有BGM（可选：比如暂停游戏时）
    /// </summary>
    public void ToggleBGMPause(bool isPause)
    {
        bgmSourceA.Pause();
        bgmSourceB.Pause();
        if (!isPause)
        {
            bgmSourceA.UnPause();
            bgmSourceB.UnPause();
        }
    }
    #endregion
}