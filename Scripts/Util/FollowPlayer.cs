using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{

    private Transform player;
    private float offset_x, offset_y;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Consts.Player).transform;
        offset_x = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.4f, Screen.height * 0.4f, 0)).x - player.position.x;
        offset_y = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.4f, Screen.height * 0.4f, 0)).y - player.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            float x = Mathf.Lerp(Camera.main.transform.localPosition.x, player.position.x /*+ Mathf.Abs(offset_x)*/, 1.0f);
            float y = Mathf.Lerp(Camera.main.transform.localPosition.y, player.position.y + Mathf.Abs(offset_y), 1.0f);
            //Debug.Log(x + " " + player.position.x + " " + Camera.main.transform.position.x + " " + Camera.main.transform.localPosition);
            Debug.Log(offset_x + " " + offset_y + " " + Globals.Instance.curLevel + " " + Consts.camEndPos[Globals.Instance.curLevel]);

            x = Mathf.Clamp(x, 0, Consts.camEndPos[Globals.Instance.curLevel]);//控制摄像机移动范围
            y = Mathf.Clamp(y, 0, 3);//控制摄像机移动范围

            Camera.main.transform.localPosition = new Vector3(x,
                Camera.main.transform.localPosition.y,
                //y,
                Camera.main.transform.localPosition.z);

        }

    }
}
