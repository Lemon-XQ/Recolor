using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CompleteUI : MonoBehaviour
{
    //[Header("胜利总分")]
    //public Text allScoreText;
    //[Header("失败总分")]
    //public Text allScoreText_over;
    //[Header("提示文本")]
    //public GameObject hintText;
    [Header("游戏胜利返回按钮")]
    public Button winBackBtn;
    public Button[] levelBtns;

    //[Header("游戏胜利重来按钮")]
    //public Button winResetButton;
    //[Header("游戏结束返回按钮")]
    //public Button overBackBtn;
    //[Header("游戏结束重来按钮")]
    //public Button overResetBtn;

    private int needUpdateIndex;
    private bool needUpdate = false;

    void Start()
    {
        // 注册按钮事件
        winBackBtn.onClick.AddListener(delegate { OnBackBtnClick(); });
        //overBackBtn.onClick.AddListener(delegate { OnBackBtnClick(); });
        //winResetButton.onClick.AddListener(delegate { OnResetButtonClick(); });
        //overResetBtn.onClick.AddListener(delegate { OnResetButtonClick(); });

        for (int i = 0; i < Consts.LevelNum; i++)
        {
            Image img = levelBtns[i].transform.Find("Image").GetComponent<Image>();
            Color color = img.color;
            int passLevelNum = PlayerPrefs.HasKey("Level") ? PlayerPrefs.GetInt("Level") + 1 : 0;
            if (i <= passLevelNum)
            {
                int index = i;
                levelBtns[i].onClick.AddListener(delegate { OnLevelBtnClick(index); });
                img.color = new Color(color.r, color.g, color.b, 1);
                img.sprite = Resources.Load<Sprite>("Sprites/level" + index.ToString());
            }
            else
            {
                //levelBtns[i].transform.Find("Image").GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.5f);
            }
        }
    }

    void OnBackBtnClick()
    {
        Time.timeScale = 1;
        SceneMgr.Instance.LoadScene(Consts.Scene_Menu);
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);

        AudioMgr.Instance.PlayMusic(Consts.Audio_BGM);

        this.gameObject.SetActive(false);
    }

    void OnLevelBtnClick(int levelIndex)
    {
        GameManager.Instance.gameState = GameState.Start;
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);

        Globals.Instance.curLevel = levelIndex;

        int sceneIndex = levelIndex + 1; // 0为Main主菜单
        Debug.Log(sceneIndex);
        
        SceneMgr.Instance.LoadSceneAsnc(sceneIndex);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (needUpdate)
        {
            Image img = levelBtns[needUpdateIndex].transform.Find("Image").GetComponent<Image>();
            Color color = img.color;
            //Debug.Log(needUpdateIndex+" "+color.a);
            //Debug.Log(levelBtns[needUpdateIndex].transform.Find("Image").name);
            if (Mathf.Abs(color.a - 1) > 0.01)
            {
                float step = Time.deltaTime * Consts.ColorChangeSpeed;
                img.color = new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, 1, step));
            }else
            {
                needUpdate = false;
            }

        }
    }

    public void UpdateLevel(int index)
    {
        needUpdateIndex = index;
        needUpdate = true;

        levelBtns[needUpdateIndex].onClick.AddListener(delegate { OnLevelBtnClick(needUpdateIndex); });
        StartCoroutine(Wait());
        Image img = levelBtns[needUpdateIndex].transform.Find("Image").GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>("Sprites/level" + index.ToString());
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }


    //void OnResetButtonClick()
    //{
    //    Time.timeScale = 1;
    //    // 重新加载场景
    //    SceneMgr.Instance.LoadScene(Consts.Scene_Main);
    //    AudioMgr.Instance.PlayEffect(Consts.Audio_Click);

    //    AudioMgr.Instance.PlayMusic(Consts.Audio_BGM);
    //}

    /// <summary>
    /// 游戏结束更新分数
    /// </summary>
    //public void UpdateScore()
    //{
    //    int score = GameManager.Instance.score;
    //    //Debug.Log(score);
    //    //if (Globals.Instance.mode == 1)// 无尽模式下失败更新分数
    //    //{
    //    //    allScoreText_over.transform.parent.gameObject.SetActive(true);
    //    //    allScoreText_over.text = score.ToString();
    //    //}else
    //    //{
    //    //    allScoreText_over.transform.parent.gameObject.SetActive(false);
    //    //}

    //    allScoreText.text = score.ToString();

    //}


}
