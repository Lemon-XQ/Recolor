using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    //[Header("返回按钮")]
    //public Button backBtn;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Consts.Player).GetComponent<Player>();
        //backBtn.onClick.AddListener(delegate { OnBackBtnClick(); });
    }

    //void OnBackBtnClick()
    //{
    //    SceneMgr.Instance.LoadScene(Consts.Scene_Menu);
    //    AudioMgr.Instance.PlayEffect(Consts.Audio_Click);

    //    AudioMgr.Instance.PlayMusic(Consts.Audio_BGM);
    //}

}
