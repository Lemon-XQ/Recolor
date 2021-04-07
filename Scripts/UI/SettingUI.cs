using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingUI : MonoBehaviour
{
    [Header("设置按钮")]
    public Button settingBtn;
    [Header("返回按钮")]
    public Button backBtn;
    [Header("保存按钮")]
    public Button saveBtn;
    [Header("继续按钮")]
    public Button continueBtn;
    [Header("重新开始按钮")]
    public Button resetBtn;
    [Header("设置面板")]
    public GameObject settingPanel;
    public Text saveText;

    //private CameraBlur cameraBlur;
    private GameObject player;

    void Start()
    {
        // 注册按钮事件
        settingBtn.onClick.AddListener(delegate { OnSettingBtnClick(); });
        backBtn.onClick.AddListener(delegate { OnBackBtnClick(); });
        saveBtn.onClick.AddListener(delegate { OnSaveBtnClick(); });
        continueBtn.onClick.AddListener(delegate { OnContinueBtnClick(); });
        resetBtn.onClick.AddListener(delegate { OnResetBtnClick(); });

        //cameraBlur = Camera.main.GetComponent<CameraBlur>();
        //cameraBlur.enabled = false;

        player = GameObject.FindGameObjectWithTag(Consts.Player);//.GetComponent<Player>();
    }

    void OnSettingBtnClick()
    {
        // 暂停游戏
        //cameraBlur.enabled = true;
        Time.timeScale = 0;
        settingPanel.SetActive(true);
        settingPanel.transform.SetAsLastSibling();// 置于顶层
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    void OnBackBtnClick()
    {
        //cameraBlur.enabled = false;
        Time.timeScale = 1;
        // 返回主界面
        SceneMgr.Instance.LoadScene(Consts.Scene_Menu);
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    void OnSaveBtnClick()
    {
        //cameraBlur.enabled = false;
        Time.timeScale = 1;
        // 保存进度
        PlayerPrefs.SetFloat("StartPos_X", player.transform.localPosition.x);
        PlayerPrefs.SetFloat("StartPos_Y", player.transform.localPosition.y);
        Globals.Instance.startPos = player.transform.localPosition;
        PlayerPrefs.SetInt("Level", Globals.Instance.curLevel);

        Debug.Log("Saved");
        StartCoroutine(ShowInfo());
        //settingPanel.SetActive(false);
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    IEnumerator ShowInfo()
    {
        saveText.text = "保存中...";
        yield return new WaitForSeconds(1f);
        saveText.text = "保存成功";
        yield return new WaitForSeconds(0.5f);
        saveText.text = "";
        settingPanel.SetActive(false);
    }

    void OnContinueBtnClick()
    {
        // 开始游戏
        //cameraBlur.enabled = false;
        Time.timeScale = 1;
        settingPanel.SetActive(false);
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    void OnResetBtnClick()
    {
        // 重新开始游戏
        //cameraBlur.enabled = false;
        Time.timeScale = 1;
        // 重新加载场景
        int sceneIndex = 1 + Globals.Instance.curLevel/* + Globals.Instance.chapter * Consts.Max_Level_Count*/;
        SceneMgr.Instance.LoadScene(sceneIndex);
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnSettingBtnClick();
        }
    }
}
