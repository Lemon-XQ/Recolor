using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("开始按钮")]
    public Button startBtn;

    [Header("关卡面板")]
    public GameObject levelPanel;
    [Header("关卡列表")]
    public Button[] levelBtns;
    [Header("关卡关闭按钮")]
    public Button levelClose;

    [Header("帮助按钮")]
    public Button helpBtn;
    [Header("帮助面板")]
    public GameObject helpPanel;

    [Header("选项按钮")]
    public Button optionBtn;
    [Header("选项关闭按钮")]
    public Button optionClose;
    [Header("选项面板")]
    public GameObject optionPanel;

    [Header("退出按钮")]
    public Button quitBtn;

    [Header("音量滑块")]
    public Slider musicSlider;
    [Header("音效滑块")]
    public Slider effectSlider;

    [Header("背景介绍面板")]
    public GameObject introPanel;
    [Header("背景继续按钮")]
    public Button continueButton;

    [Header("背景图")]
    public Image bgImage;
    [Header("标题图")]
    public Image titleImage;
    //[Header("雨滴特效")]
    //public RainScript2D rainEffect;

    void Start()
    {
        // 背景设定
        Debug.Log("111"+PlayerPrefs.HasKey("Level") + " " + PlayerPrefs.GetInt("Level") + " " + Consts.LevelNum);
        if (PlayerPrefs.HasKey("Level") && PlayerPrefs.GetInt("Level") == Consts.LevelNum-1)
            bgImage.sprite = Resources.Load<Sprite>("Sprites/bg_color");
        else
            bgImage.sprite = Resources.Load<Sprite>("Sprites/bg");

        // 注册按钮事件
        startBtn.onClick.AddListener(delegate { OnStartBtnClick(); });
        optionBtn.onClick.AddListener(delegate { OnOptionBtnClick(); });
        optionClose.onClick.AddListener(delegate { OnOptionCloseClick(); });
        quitBtn.onClick.AddListener(delegate { OnQuitBtnClick(); });
        helpBtn.onClick.AddListener(delegate { OnHelpBtnClick(); });
        //continueButton.onClick.AddListener(delegate { OnContinueBtnClick(); });
        for(int i = 0; i < Consts.LevelNum; i++)
        {
            Image img = levelBtns[i].transform.Find("Image").GetComponent<Image>();
            Color color = img.color;
            if (i <= Globals.Instance.passLevel)
            {
                int index = i;
                levelBtns[i].onClick.AddListener(delegate { OnLevelBtnClick(index); });
                img.color = new Color(color.r, color.g, color.b, 1f);
                img.sprite = Resources.Load<Sprite>("Sprites/level" + index.ToString());
            }
            else
            {
                //img.color = new Color(color.r, color.g, color.b, 0.5f);
            }
        }
        levelClose.onClick.AddListener(delegate { OnLevelCloseClick(); });

        // 注册滑块事件
        musicSlider.onValueChanged.AddListener(delegate { OnMusicSliderChange(); });
        effectSlider.onValueChanged.AddListener(delegate { OnEffectSliderChange(); });

        // 读取音量存档 默认0.5
        float musicVolume = PlayerPrefs.GetFloat(Consts.Music_Volume, 0.5f);
        musicSlider.value = musicVolume;
        AudioMgr.Instance.SetMusicVolume(musicVolume);

        // 读取音效存档 默认0.5
        float effectVolume = PlayerPrefs.GetFloat(Consts.Effect_Volume, 0.5f);
        effectSlider.value = effectVolume;
        AudioMgr.Instance.SetEffectVolume(effectVolume);

        // 开启雨滴特效
        //rainEffect.RainIntensity = 0.3f;

 

    }

    void Update()
    {
        if (Mathf.Abs( titleImage.color.a - 1)>0.1f)
        {
            float step = Time.deltaTime * Consts.ColorChangeSpeed;
            titleImage.color = new Color(titleImage.color.r, titleImage.color.g, titleImage.color.b,
                Mathf.Lerp(titleImage.color.a, 1, step));
        }
    }

    void LoadScene(int index)
    {
        SceneMgr.Instance.LoadSceneAsnc(index);
        //this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 继续
    /// </summary>
    void OnContinueBtnClick()
    {
        introPanel.SetActive(false);

        // 雨停
        //rainEffect.RainIntensity = 0;

        // 进入关卡选择
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
        levelPanel.SetActive(true);

        Globals.Instance.hasLoadIntro = true;
        PlayerPrefs.SetInt("Has_LoadIntro", 1);
    }

    /// <summary>
    /// 开始
    /// </summary>
    void OnStartBtnClick()
    {        
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);

        if (!Globals.Instance.hasLoadIntro)
        {
            introPanel.SetActive(true);
        }
        else
        {
            levelPanel.SetActive(true);
        }
        //LoadScene(Consts.Scene_Main);
    }

    /// <summary>
    /// 关卡
    /// </summary>
    void OnLevelBtnClick(int levelIndex)
    {
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
        levelPanel.SetActive(false);

        Globals.Instance.curLevel = levelIndex;

        // 显示关卡预览图
        //levelImg.sprite = Resources.Load("sprite/level" + (chapterIndex * Consts.Max_Level_Count + levelIndex), new Sprite().GetType()) as Sprite;
        //levelIntro.text = ((TextAsset)Resources.Load("introText/" + chapterIndex + "-" + levelIndex)).text;
        int sceneIndex = levelIndex + 1; // 0为Main主菜单
        Debug.Log(sceneIndex);

        //if (!Globals.Instance.hasFinishGuide) // 未完成引导
        //{
        //    //TODO 引导
        //    GuideManager.Instance.BeginGuide();
        //}
        //else
        {
            LoadScene(sceneIndex);
        }
    }

    void OnLevelCloseClick()
    {
        levelPanel.SetActive(false);
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    /// <summary>
    /// 帮助
    /// </summary>
    void OnHelpBtnClick()
    {
        helpPanel.SetActive(true);
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    /// <summary>
    /// 选项
    /// </summary>
    void OnOptionBtnClick()
    {
        optionPanel.SetActive(true);
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    void OnOptionCloseClick()
    {
        optionPanel.SetActive(false);
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    /// <summary>
    /// 退出
    /// </summary>
    void OnQuitBtnClick()
    {
        Application.Quit();
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    /// <summary>
    /// 音量控制
    /// </summary>
    void OnMusicSliderChange()
    {
        float value = musicSlider.value;
        AudioMgr.Instance.SetMusicVolume(value);
        PlayerPrefs.SetFloat(Consts.Music_Volume, value);
    }

    /// <summary>
    /// 音效控制
    /// </summary>
    void OnEffectSliderChange()
    {
        float value = effectSlider.value;
        AudioMgr.Instance.SetEffectVolume(value);
        PlayerPrefs.SetFloat(Consts.Effect_Volume, value);
    }
}
