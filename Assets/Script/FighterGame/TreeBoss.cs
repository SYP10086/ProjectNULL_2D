using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TreeBoss : MonoBehaviour
{
    // ===================== 基础属性配置 =====================
    [Header("基础属性")]
    public float maxHealth = 200f;          // 最大生命值
    private float currentHealth;           // 当前生命值

    [Header("范围配置")]
    public float wakeRange = 4f;           // 苏醒范围（比攻击范围大）
    public float attackRange = 4f;         // 普通攻击范围
    public float normalAttackDamage = 10f; // 普通攻击伤害

    [Header("攻击冷却")]
    public float normalAttackCD = 2f;      // 普通攻击冷却时间
    private float normalAttackTimer;       // 普通攻击冷却计时器

    [Header("动画配置")]
    public float attackDelay = 0.5f;       // 攻击动画播放到伤害帧的延迟（根据动画时长调整）
    public float warningDuration = 1f;     // 攻击预警持续时间

    [Header("预警预制体")]
    public GameObject warningSectorPrefab; // 普通攻击扇形预警预制体

    // ===================== 状态与组件 =====================
    private bool isWakeUp = false;         // 是否已苏醒
    private bool isDead = false;           // 是否已死亡
    private bool isAttacking = false;      // 是否正在攻击中
    private Transform player;              // 玩家Transform缓存
    private Animator anim;                 // 动画组件缓存

    void Start()
    {
        // 初始化生命值与冷却
        currentHealth = maxHealth;
        normalAttackTimer = 0;

        // 查找玩家（确保玩家Tag为Player）
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("未找到玩家！请给玩家设置Tag为Player");
        }

        // 缓存动画组件
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Boss未挂载Animator组件！");
        }

        // 初始化Animator参数
        if (anim != null)
        {
            anim.SetBool("IsPlayerInAttackRange", false);
            anim.SetBool("IsDead", false);
        }
    }

    void Update()
    {
        // 死亡/无玩家时停止所有逻辑
        if (player == null || isDead) return;

        // 攻击冷却计时更新
        if (normalAttackTimer > 0)
        {
            normalAttackTimer -= Time.deltaTime;
        }

        // 计算玩家与Boss的距离
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ===================== 苏醒逻辑 =====================
        if (!isWakeUp && distanceToPlayer <= wakeRange)
        {
            TriggerWakeAnimation(); // 触发苏醒动画
        }

        // ===================== 待机/攻击逻辑（仅苏醒后） =====================
        if (isWakeUp)
        {
            //使boss面向玩家
            FlipToFacePlayer();
            // 更新「玩家是否在攻击范围」的Animator布尔参数
            bool isInAttackRange = distanceToPlayer <= attackRange;
            anim.SetBool("IsPlayerInAttackRange", isInAttackRange);

            // 玩家在攻击范围 + 冷却完成 + 未攻击中 → 触发普通攻击
            if (isInAttackRange && normalAttackTimer <= 0 && !isAttacking)
            {
                StartCoroutine(NormalAttackCoroutine());
            }
        }
        if(currentHealth <= 0)
        {
            TriggerDeathAnimation();
        }
        Debug.Log("Boss当前生命值"+currentHealth);

        
        
    }

    // ===================== 动画触发核心函数 =====================
    /// <summary>
    /// 触发苏醒动画
    /// </summary>
    private void TriggerWakeAnimation()
    {
        isWakeUp = true;
        if (anim != null)
        {
            anim.SetTrigger("TriggerWake"); // 触发苏醒动画（对应Animator的Trigger参数）
            Debug.Log("Boss苏醒，播放苏醒动画");
        }
    }

    /// <summary>
    /// 触发普通攻击动画
    /// </summary>
    private void TriggerAttackAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger("TriggerAttack"); // 触发攻击动画（对应Animator的Trigger参数）
            Debug.Log("播放普通攻击动画");
        }
    }

    /// <summary>
    /// 触发死亡动画
    /// </summary>
    private void TriggerDeathAnimation()
    {
        isDead = true;
        if (anim != null)
        {
            anim.SetBool("IsDead", true);    // 标记死亡状态（Animator布尔参数）
            anim.SetTrigger("TriggerDeath"); // 触发死亡动画（Animator Trigger参数）
            Debug.Log("Boss死亡，播放死亡动画");
        }

        // 死亡后禁用碰撞和攻击逻辑
        GetComponent<Collider2D>().enabled = false;
        isAttacking = false;
    }

    // ===================== 普通攻击核心逻辑 =====================
    /// <summary>
    /// 普通攻击协程（预警+动画+伤害检测）
    /// </summary>
    private IEnumerator NormalAttackCoroutine()
    {
        isAttacking = true;
        normalAttackTimer = normalAttackCD; // 重置冷却

        // 1. 计算朝向玩家的方向和角度（仅用于预警，Boss本体不旋转）
        Vector2 attackDirection = GetDirectionToPlayer();//boss朝向玩家的方向
        float attackAngle = GetAngleToPlayer();

        // 2. 生成扇形预警
        GameObject warning = Instantiate(warningSectorPrefab, transform.position, Quaternion.Euler(0, 0, attackAngle));//（预警预制体，boss的位置，旋转角度）
        CanvasGroup cg = warning.GetComponent<CanvasGroup>();

        // 3. 渐变显示预警
        if (cg != null)
        {
            cg.alpha = 0;
            float t = 0;
            while (t < 0.5f)
            {
                t += Time.deltaTime;
                cg.alpha = Mathf.Lerp(0, 1, t / 0.5f);//在0.5秒内，透明度从0到1
                yield return null;
            }
        }

        // 4. 预警持续时间
        yield return new WaitForSeconds(warningDuration);//预警持续1秒

        // 5. 渐变隐藏预警
        if (cg != null)
        {
            float t = 0;
            while (t < 0.2f)
            {
                t += Time.deltaTime;
                cg.alpha = Mathf.Lerp(1, 0, t / 0.2f);//在0.2秒内，透明度从1到0
                yield return null;//让当前协程暂停执行，等到下一帧再继续执行后续代码。
            }
        }
        Destroy(warning);//删除预警信息

        // 6. 触发普通攻击动画
        TriggerAttackAnimation();

        // 7. 等待动画播放到伤害帧（attackDelay需匹配动画时长）
        yield return new WaitForSeconds(attackDelay);//伤害帧数在0.5s左右（合理）

        // 8. 普通攻击伤害检测（扇形范围）
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange * 0.8f, LayerMask.GetMask("Player"));//以当前物体位置为圆心，检测attackRange * 0.8f为半径的圆里，所有Layer层的“Player”的碰撞体
        foreach (var hitCollider in hitColliders)
        {
            PlayerMain playerMain = hitCollider.GetComponent<PlayerMain>();
            if (playerMain != null)
            {
                playerMain.HealthLose(normalAttackDamage);
                Debug.Log($"玩家受到{normalAttackDamage}点普通攻击伤害");
                break;
            }
        }

        // 9. 攻击动画播放完成后恢复状态（等待动画剩余时长）
        yield return new WaitForSeconds(1.5f); // 可根据攻击动画总时长调整
        isAttacking = false;
    }

    // ===================== 辅助函数 =====================
    /// <summary>
    /// 计算朝向玩家的归一化方向（仅用于攻击/预警）
    /// </summary>
    private Vector2 GetDirectionToPlayer()
    {
        if (player == null) return Vector2.right;
        return (player.position - transform.position).normalized;
    }

    /// <summary>
    /// 计算朝向玩家的角度（用于预警旋转）
    /// </summary>
    private float GetAngleToPlayer()
    {
        Vector2 direction = GetDirectionToPlayer();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // 偏移适配2D素材
        return angle;
    }

    // ===================== 生命值与死亡 =====================
    /// <summary>
    /// 受击函数（供玩家攻击调用）
    /// </summary>
    /// <param name="damage">受到的伤害值</param>
    public void TakeDamage(float damage)
    {
        if (isDead) return; // 死亡后不再受击

        currentHealth = Mathf.Max(0, currentHealth - damage);
        Debug.Log($"Boss受到{damage}点伤害，剩余生命值：{currentHealth}");

        // 生命值为0触发死亡
        if (currentHealth <= 0)
        {
            TriggerDeathAnimation();
        }
    }

    // ===================== Gizmos调试（可选） =====================
    void OnDrawGizmosSelected()
    {
        // 绘制苏醒范围（黄色）
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wakeRange);

        // 绘制攻击范围（红色）
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 绘制朝向玩家的方向（蓝色）
        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }

    private void FlipToFacePlayer()
    {
        if (player == null) return;

        // 获取玩家相对于Boss的水平位置
        float playerPositionX = player.position.x;
        float bossPositionX = transform.position.x;

        // 计算缩放比例（只修改X轴缩放来实现翻转）
        Vector3 newScale = transform.localScale;

        // 玩家在右侧且Boss当前面朝左（X轴缩放为正）
        if (playerPositionX > bossPositionX && newScale.x > 0)
        {
            newScale.x = -newScale.x; // 翻转朝向右侧
        }
        // 玩家在左侧且Boss当前面朝右（X轴缩放为负）
        else if (playerPositionX < bossPositionX && newScale.x < 0)
        {
            newScale.x = -newScale.x; // 翻转朝向左侧
        }

        transform.localScale = newScale;
    }

    void Hit(string a, float b)
    {
        if (a == "TreeBoss")
        {
            currentHealth -= b;
        }
    }
    TreeBoss() { Event.Attack += new MyStrFloat(Hit); }
    ~TreeBoss() { Event.Attack -= new MyStrFloat(Hit); }
}

