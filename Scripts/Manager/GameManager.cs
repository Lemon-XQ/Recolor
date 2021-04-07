using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum GameState
{
    Ready,
    Start,
    Run, // 游戏运行
    GameWin,
    GameOver,
    GameFinalWin,
    End
}

public enum ColorState
{
    None,
    Painting,
    Erasing
}

public class GameManager : GameSingleton<GameManager>
{
    [Header("游戏状态")]
    public GameState gameState;

    [Header("上色状态")]
    public ColorState colorState;
    public Color curColor;

    [Header("UI管理类")]
    public GameObject uiMgr;

    [Header("游戏胜利界面")]
    public GameObject gameWinPanel;
    [Header("游戏结束界面")]
    public GameObject gameOverPanel;

    //private CompleteUI winUI;
    
    private float _timer = 0.0f; // 计时
    public int timer
    {
        get { return (int)Mathf.Ceil(_timer); }
    }

    private int _score;
    public int score
    {
        get { return _score; }
        set { _score = value; }
    }

    private Canvas _canvas;
    private bool isWin = false;

    void Start()
    {
        uiMgr = GameObject.Find("UIMgr");
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }
    
    void Update()
    {
        switch (gameState)
        {
            case GameState.Ready:
                Debug.Log("GameState.Ready");

                _timer = 0;
                _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                colorState = ColorState.None;
                isWin = false;

                break;
            case GameState.Start:
                Debug.Log("GameState.Start");
                isWin = false;
                // 计时
                //_timer += Time.deltaTime;

                // 更新UI
                //uiMgr.GetComponent<MainUI>().UpdateInfo();

                // 压力满时死亡
                //if (pressure >= Consts.Max_Pressure)
                //{
                //    gameState = GameState.GameOver;
                //    break;
                //}

                //if (Globals.Instance.mode == 0)// 普通模式
                //{
                //    // 到最大时间且仍存活 / 压力值为0时胜利
                //    if (_timer >= Consts.Max_Time || pressure <= 0) 
                //        gameState = GameState.GameWin; 
                //}
                //else// 无尽模式
                //{
                //    // 压力值为0时胜利
                //    if (pressure <= 0)
                //        gameState = GameState.GameWin;
                //}

                break;
            case GameState.Run:

                // NONE
                Debug.Log("GameState.Run");

                break;
            case GameState.GameWin:
                Debug.Log("GameState.GameWin");
                gameState = GameState.End;
                colorState = ColorState.None;

                if (!isWin) // 只处理一次
                {
                    isWin = true;
                    gameWinPanel.SetActive(true);
                    gameWinPanel.transform.SetAsLastSibling();

                    // update level 只保存最大的
                    //Globals.Instance.level++;
                    if (!PlayerPrefs.HasKey("Level") ||
                        (PlayerPrefs.HasKey("Level") && Globals.Instance.curLevel > PlayerPrefs.GetInt("Level")))
                    PlayerPrefs.SetInt("Level", Globals.Instance.curLevel);

                    // update ui
                    uiMgr.GetComponent<CompleteUI>().UpdateLevel(Globals.Instance.curLevel + 1);
                }
                else
                {
                    // 渐变
                    //leve
                }
                // TODO 过关特效
                //StartCoroutine(UpdateLevel());
                // TODO 选关界面

                //gameWinPanel.transform.SetAsLastSibling();// 置于顶层
                //// 让人和地板出现在背景里
                ////GameObject.Find(Consts.Player).transform.SetAsLastSibling();
                ////GameObject.Find(Consts.Wall).transform.SetAsLastSibling();
                //// 雨出现在背景里（粒子效果出现在UI上）
                //_canvas.renderMode = RenderMode.ScreenSpaceCamera;
                //_canvas.worldCamera = Camera.main;

                //// 播放胜利音乐
                //AudioMgr.Instance.PlayEffect(Consts.Audio_Win);

                //// 雨停
                //RainManager.Instance.rain_stop = true;

                // 保存分数（即剩余时间+压力，用时越短且压力越小者分数越高）
                //if (Globals.Instance.mode == 0)
                //    _score = Consts.Max_Time - (timer - 1) + Consts.Max_Pressure - pressure;
                //else
                //    _score = timer - 1 + Consts.Max_Pressure - pressure;
                //string info = Globals.Instance.username + " " + Globals.Instance.mode.ToString() + " " + _score.ToString();
                //Util.WriteIntoTxt(info);                

                // 更新UI
                //uiMgr.GetComponent<CompleteUI>().UpdateScore();

                break;
            case GameState.GameFinalWin:
                gameState = GameState.End;

                if (!PlayerPrefs.HasKey("Level") ||
                (PlayerPrefs.HasKey("Level") && Globals.Instance.curLevel >= PlayerPrefs.GetInt("Level")))
                    PlayerPrefs.SetInt("Level", Globals.Instance.curLevel);

                gameWinPanel.SetActive(true);
                gameWinPanel.transform.SetAsLastSibling();

                AudioMgr.Instance.PlayMusic(Consts.Audio_FinalWin);

                break;
            case GameState.GameOver:
                Debug.Log("GameState.GameOver");
                gameState = GameState.End;
                colorState = ColorState.None;

                // TODO 死亡特效 渐隐场景再出现?
                StartCoroutine(Reborn()); // 重置场景 加载上一个保存点

                //gameOverPanel.SetActive(true);
                //gameOverPanel.transform.SetAsLastSibling();// 置于顶层
                //// 让人和地板出现在背景里
                //GameObject.Find(Consts.Player).transform.SetAsLastSibling();
                //GameObject.Find(Consts.Wall).transform.SetAsLastSibling();
                //// 雨出现在背景里
                //_canvas.renderMode = RenderMode.ScreenSpaceCamera;
                //_canvas.worldCamera = Camera.main;

                //// 销毁玩家
                //Destroy(GameObject.Find(Consts.Player));
                //Rigidbody2D playerRigidbody2D = GameObject.FindGameObjectWithTag(Consts.Player).GetComponent<Rigidbody2D>();
                //Destroy(playerRigidbody2D);

                //// 生成失败特效（雨变大）
                //RainManager.Instance.rain_heavy = true;

                // 播放失败音乐
                AudioMgr.Instance.PlayEffect(Consts.Audio_HeroDie);

                break;
            case GameState.End:
                Debug.Log("GameState.End");
                colorState = ColorState.None;

                //if (gameWinPanel.activeInHierarchy)
                //{
                //    // 胜利特效（日出效果+雨停）200/255 200/255 170/255
                //    Color color = gameWinPanel.GetComponent<Image>().color;
                //    float step = Consts.ColorChangeSpeed * Time.deltaTime;
                //    //gameWinPanel.GetComponent<Image>().color = new Color(Mathf.Lerp(color.r, 200 / 255.0f, step), Mathf.Lerp(color.g, 200 / 255.0f, step), Mathf.Lerp(color.b, 170 / 255.0f, step), 1);

                //    //RainManager.Instance.rain_stop = true;
                //}

                break;
            default:
                break;
        }
    }

    IEnumerator Reborn()
    {
        GameObject.FindGameObjectWithTag(Consts.Player).GetComponent<PlayerAnimation>().DieState();
        yield return new WaitForSeconds(2f);
        //yield return new WaitForSeconds(0.5f);
        // 重置场景 加载上一个保存点
        foreach(ResetObject go in FindObjectsOfType<ResetObject>())
        {
            go.CustomReset();
        }

        GameObject.FindGameObjectWithTag(Consts.Player).GetComponent<PlayerAnimation>().IdleState();
        GameObject.FindGameObjectWithTag(Consts.Player).transform.localPosition = Globals.Instance.startPos;
        GameObject.FindGameObjectWithTag(Consts.Player).GetComponent<Player>().IsDead = false;
        gameState = GameState.Start;

    }

    //IEnumerator UpdateLevel()
    //{
    //    yield return new WaitForSeconds(1f);
    //    // 重置场景 加载上一个保存点
    //    GameObject.FindGameObjectWithTag(Consts.Player).transform.localPosition = Globals.Instance.startPos;
    //    GameObject.FindGameObjectWithTag(Consts.Player).GetComponent<PlayerAnimation>().IdleState();
    //    GameObject.FindGameObjectWithTag(Consts.Player).GetComponent<Player>().IsDead = false;
    //    gameState = GameState.Start;

    //}
}
