using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    public GameObject[] anim_list;

    private int curIndex = 0;
    private Image[] img_list;
    public GameObject[] text_list;

    private bool flag = false;

    // Start is called before the first frame update
    void Start()
    {
        img_list = new Image[anim_list.Length];
        //text_list = new GameObject[anim_list.Length];
        for (int i = 0; i< anim_list.Length;i++)
        {
            img_list[i] = anim_list[i].GetComponentInChildren<Image>();
            //text_list[i] = anim_list[i].GetComponentInChildren<Text>().gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (curIndex < anim_list.Length)
        {
            Color color = img_list[curIndex].color;
            float step = Consts.ColorChangeSpeed * Time.deltaTime;
            if (Mathf.Abs(color.a - 1) > 0.1f)
            {
                img_list[curIndex].color = new Color(img_list[curIndex].color.r, img_list[curIndex].color.g, img_list[curIndex].color.b,
                    Mathf.Lerp(img_list[curIndex].color.a, 1, step));
            }else
            {
                Debug.Log(color.a);
                img_list[curIndex].color = new Color(img_list[curIndex].color.r, img_list[curIndex].color.g, img_list[curIndex].color.b,
                1);
            }
            if (Mathf.Abs(color.a - 1) < 0.5f)
            {
                if (!flag)
                {
                    text_list[curIndex].SetActive(true);
                    flag = true;
                    StartCoroutine(Wait());
                }
            }
        }else
        {
            if (Input.anyKeyDown)
            {
                SceneMgr.Instance.LoadScene(Consts.Scene_Menu);
                this.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.5f);
        //anim_list[curIndex].SetActive(false);
        if (curIndex != anim_list.Length - 1)
            text_list[curIndex].SetActive(false);
        curIndex++;
        Debug.Log(curIndex);
        if (curIndex < anim_list.Length)
            anim_list[curIndex].SetActive(true);
        flag = false;
    }
}
