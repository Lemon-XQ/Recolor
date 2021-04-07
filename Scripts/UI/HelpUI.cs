using UnityEngine;
using UnityEngine.UI;

public class HelpUI : MonoBehaviour
{
    [Header("关闭按钮")]
    public Button closeBtn;
    [Header("确认按钮")]
    public Button confirmBtn;
    [Header("登录面板")]
    public GameObject signinPanel;
    //[Header("背景介绍面板")]
    //public GameObject introPanel;

    void Start()
    {
        confirmBtn.onClick.AddListener(delegate { OnConfirmBtnClick(); });
        closeBtn.onClick.AddListener(delegate { gameObject.SetActive(false); AudioMgr.Instance.PlayEffect(Consts.Audio_Click); });
    }

    void OnConfirmBtnClick()
    {
        this.gameObject.SetActive(false);
        //if (!Globals.Instance.signIn)
        //{
        //    signinPanel.SetActive(true);
        //}
        //introPanel.SetActive(true);// 加载背景介绍
        // 进入游戏
        //SceneMgr.Instance.LoadSceneAsnc(Consts.Scene_Main);
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }
}
