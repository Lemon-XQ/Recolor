using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TypeWriterEffect : MonoBehaviour
{
    //[Header("继续按钮")]
    //public GameObject continueButton;
    [Header("打字时间间隔")]
    public float charsPerSecond = 0.2f;

    private string words;// 保存需要显示的文字
    private bool isActive = false;
    private float timer;// 计时器
    private Text myText;
    private int currentPos = 0;// 当前打字位置

    // Use this for initialization
    void Start()
    {
        timer = 0;
        isActive = true;
        //charsPerSecond = Mathf.Max(0.2f, charsPerSecond);
        myText = GetComponent<Text>();
        words = myText.text;
        myText.text = "";// 获取Text的文本信息，保存到words中，然后动态更新文本显示内容，实现打字机的效果

        //if (Globals.Instance.hasLoadIntro && continueButton != null) continueButton.SetActive(true);
        StartEffect();
    }

    // Update is called once per frame
    void Update()
    {
        OnStartWriter();
    }

    public void StartEffect()
    {
        isActive = true;
    }
    /// <summary>
    /// 执行打字任务
    /// </summary>
    void OnStartWriter()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            // 判断计时器时间是否到达
            if (timer >= charsPerSecond)
            {                
                timer = 0;
                currentPos++;
                myText.text = words.Substring(0, currentPos);// 刷新文本显示内容

                //Debug.Log(currentPos.ToString() + " " + words.Length);
                if (currentPos >= words.Length)
                {
                    OnFinish();
                }
            }

            if (Input.GetMouseButtonDown(0))
                OnFinish();

        }
    }
    /// <summary>
    /// 结束打字，初始化数据
    /// </summary>
    void OnFinish()
    {
        isActive = false;
        timer = 0;
        currentPos = 0;
        myText.text = words;
        //GetComponentInParent<AudioSource>().Stop();

        //if (continueButton != null)
        //{
        //    显示按钮
        //    continueButton.SetActive(true);
        //}
    }

}
