using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class StarfieldBackground : MonoBehaviour
{
    [Header("粒子系统设置")]
    public float starLifetime = 100f;          // 粒子存活时间
    public float startSpeed = 0f;              // 初始速度
    public float startSizeMin = 0.05f;         // 最小粒子大小
    public float startSizeMax = 0.3f;          // 最大粒子大小
    public float emissionRate = 5f;            // 每秒发射的粒子数量

    [Header("颜色设置")]
    public Color startColor = Color.white;     // 粒子开始颜色
    public Color endColor = Color.gray;        // 粒子结束颜色

    [Header("形状设置")]
    public ParticleSystemShapeType shapeType = ParticleSystemShapeType.Box;
    public Vector3 shapeScale = new Vector3(50, 30, 0); // 粒子发射区域大小

    [Header("高级设置")]
    public bool useRandomColor = false;        // 是否使用随机颜色
    public bool loopSystem = true;             // 是否循环
    public bool playOnAwake = true;            // 是否唤醒时播放

    private ParticleSystem starParticles;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.ShapeModule shapeModule;
    private ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule;

    void Start()
    {
        InitializeParticleSystem();
        SetupParticleSystem();
    }

    private void InitializeParticleSystem()
    {
        // 获取或添加粒子系统组件
        starParticles = GetComponent<ParticleSystem>();

        // 获取粒子系统的各个模块
        mainModule = starParticles.main;
        emissionModule = starParticles.emission;
        shapeModule = starParticles.shape;
        colorOverLifetimeModule = starParticles.colorOverLifetime;
    }

    private void SetupParticleSystem()
    {
        // 设置主模块
        mainModule.startLifetime = starLifetime;
        mainModule.startSpeed = startSpeed;
        mainModule.startSize = new ParticleSystem.MinMaxCurve(startSizeMin, startSizeMax);
        mainModule.loop = loopSystem;
        mainModule.playOnAwake = playOnAwake;
        mainModule.maxParticles = 1000; // 最大粒子数

        // 设置发射模块
        emissionModule.rateOverTime = emissionRate;
        emissionModule.enabled = true;

        // 设置形状模块
        shapeModule.shapeType = shapeType;
        shapeModule.scale = shapeScale;

        // 设置颜色模块
        SetupColorOverLifetime();
    }

    private void SetupColorOverLifetime()
    {
        // 启用颜色随时间变化模块
        colorOverLifetimeModule.enabled = true;

        // 创建渐变
        Gradient gradient = new Gradient();

        if (useRandomColor)
        {
            // 随机颜色模式
            GradientColorKey[] colorKeys = new GradientColorKey[3];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[3];

            // 开始时的颜色和透明度
            colorKeys[0].color = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);
            colorKeys[0].time = 0.0f;
            alphaKeys[0].alpha = 0.0f;
            alphaKeys[0].time = 0.0f;

            // 中间的颜色和透明度
            colorKeys[1].color = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);
            colorKeys[1].time = 0.3f;
            alphaKeys[1].alpha = 1.0f;
            alphaKeys[1].time = 0.3f;

            // 结束时的颜色和透明度
            colorKeys[2].color = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);
            colorKeys[2].time = 1.0f;
            alphaKeys[2].alpha = 0.0f;
            alphaKeys[2].time = 1.0f;

            gradient.SetKeys(colorKeys, alphaKeys);
        }
        else
        {
            // 固定颜色模式
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[3];

            // 颜色键
            colorKeys[0].color = startColor;
            colorKeys[0].time = 0.0f;
            colorKeys[1].color = endColor;
            colorKeys[1].time = 1.0f;

            // 透明度键（淡入淡出效果）
            alphaKeys[0].alpha = 0.0f;  // 开始时完全透明
            alphaKeys[0].time = 0.0f;
            alphaKeys[1].alpha = 1.0f;  // 中间完全不透明
            alphaKeys[1].time = 0.3f;
            alphaKeys[2].alpha = 0.0f;  // 结束时完全透明
            alphaKeys[2].time = 1.0f;

            gradient.SetKeys(colorKeys, alphaKeys);
        }

        // 将渐变应用到粒子系统
        colorOverLifetimeModule.color = gradient;
    }

    [ContextMenu("重新生成粒子系统")]
    public void RegenerateParticleSystem()
    {
        // 清除现有粒子
        starParticles.Clear();

        // 重新设置粒子系统
        SetupParticleSystem();

        // 重新播放
        starParticles.Play();
    }

    [ContextMenu("暂停粒子系统")]
    public void PauseParticleSystem()
    {
        starParticles.Pause();
    }

    [ContextMenu("继续播放粒子系统")]
    public void PlayParticleSystem()
    {
        starParticles.Play();
    }

    [ContextMenu("停止粒子系统")]
    public void StopParticleSystem()
    {
        starParticles.Stop();
    }

    // 在编辑器中更新参数
#if UNITY_EDITOR
    void OnValidate()
    {
        if (Application.isPlaying && starParticles != null)
        {
            // 确保数值在合理范围内
            emissionRate = Mathf.Max(0, emissionRate);
            startSizeMin = Mathf.Max(0.01f, startSizeMin);
            startSizeMax = Mathf.Max(startSizeMin, startSizeMax);

            // 更新粒子系统参数
            if (starParticles.isPlaying)
            {
                RegenerateParticleSystem();
            }
        }
    }
#endif
}