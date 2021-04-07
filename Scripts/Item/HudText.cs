using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudText : MonoBehaviour
{
    private int _born_time;
    private Text hud_text;
    
    void Start()
    {

    }

    public void Init(Vector3 player_pos, string text, Color color)
    {
        _born_time = (int)Time.time;// GameManager.Instance.timer;
        hud_text = GetComponent<Text>();

        hud_text.transform.SetParent(GameObject.Find("Canvas").transform);
        hud_text.transform.localPosition = player_pos + new Vector3(0, 20, 0);
        hud_text.GetComponent<Text>().text = text;
        hud_text.GetComponent<Text>().color = color;
    }
    
    void Update()
    {
        // 超过3s消失（用系统时间，防止停止计时时特效一直存在）
        int delta_time = (int)Time.time/*GameManager.Instance.timer*/ - _born_time;
        if (delta_time > 3)
        {
            Destroy(this.gameObject);
        }

        // 上升
        transform.Translate(Vector2.up * Consts.HUDSpeed * Time.deltaTime);
        // 透明处理
        hud_text.color = new Color(hud_text.color.r, hud_text.color.g, hud_text.color.b, hud_text.color.a - 0.01f);        
    }
}
