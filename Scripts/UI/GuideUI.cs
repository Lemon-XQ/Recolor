using UnityEngine;
using UnityEngine.UI;

public class GuideUI : MonoBehaviour
{
    [Header("对话框面板")]
    public GameObject dialog;
    [Header("开始引导按钮")]
    public Button beginGuideBtn;
    [Header("跳过引导按钮")]
    public Button skipGuideBtn;
    [Header("完成引导按钮")]
    public Button finishGuideBtn;

    void Start()
    {
        if (!Globals.Instance.hasFinishGuide) // 未完成引导
        {
            dialog.SetActive(true);
        }
        beginGuideBtn.onClick.AddListener(delegate { OnBeginGuideBtnClick(); });
        skipGuideBtn.onClick.AddListener(delegate { OnSkipGuideBtnClick(); });
        //finishGuideBtn.onClick.AddListener(delegate { OnFinishGuideBtnClick(); });
    }

    void OnBeginGuideBtnClick()
    {
        dialog.SetActive(false);
        GuideManager.Instance.BeginGuide();
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    void OnSkipGuideBtnClick()
    {
        PlayerPrefs.SetInt("Has_FinishGuide", 1);

        this.gameObject.SetActive(false);
        GameManager.Instance.gameState = GameState.Start;
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    void OnFinishGuideBtnClick()
    {
        PlayerPrefs.SetInt("Has_FinishGuide", 1);

        this.gameObject.SetActive(false);
        GuideManager.Instance.EndGuide();
        GameManager.Instance.gameState = GameState.Start;
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

}
