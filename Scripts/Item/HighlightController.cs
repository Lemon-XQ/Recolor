using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Xml.Serialization;

public class HighlightController : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{
    //public EdgeCollider2D collider;
    public GameObject child_collider;

    private bool highlight = false;
    public Material highlightMaterial;
    private Animator anim;

    //private bool isMoving = false;
    //private bool hasPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        //highlightMaterial = GetComponent<Image>().material = new Material(Shader.Find("Custom/Highlight"));
        highlightMaterial = GetComponent<SpriteRenderer>().material = new Material(Shader.Find("Custom/Highlight"));
        //highlightMaterial = GetComponent<Image>().material;
        anim = GetComponent<Animator>();

        if (GetComponent<ResetObject>() == null && gameObject.tag!=Consts.Green_Tree)
        {
            gameObject.AddComponent<ResetObject>();
        }
    }

    /// <summary>
    /// 鼠标指向模型时触发
    /// </summary>
    //public void OnPointerEnter(PointerEventData eventData)
   public void OnMouseEnter()
   {
        Debug.Log(this.name + GameManager.Instance.colorState+" enter");

        if (GameManager.Instance.colorState != ColorState.None)
        {
            if (GameManager.Instance.colorState == ColorState.Painting)
            {
                 //开启外发光
                highlightMaterial.SetInt("_ShowOutline", 1);
                highlightMaterial.SetColor("_Color", GameManager.Instance.curColor);
           }
            else
            {
                highlightMaterial.SetInt("_Painted", 0);
                highlightMaterial.SetInt("_ShowOutline", 0);
            }

            //ho.FlashingOn(Color.green, Color.green, 1f);
            highlight = true;
            Debug.Log("highlighting");
        }
    }

    /// <summary>
    /// 鼠标离开模型时触发
    /// </summary>
    //public void OnPointerExit(PointerEventData eventData)
    public void OnMouseExit()
    {
        if (GameManager.Instance.colorState != ColorState.None)
        {
            //关闭外发光
            highlightMaterial.SetInt("_ShowOutline", 0);
            highlight = false;

            if (GameManager.Instance.colorState == ColorState.Erasing)
            {
                highlightMaterial.SetInt("_ShowOutline", 1); // 恢复原色
            }
        }
    }

    void Update()
    {
        if (highlight && Input.GetMouseButtonDown(0))
        {
            Debug.Log(this.name+ GameManager.Instance.colorState);
            ////highlightMaterial.SetInt("_ShowOutline", 0);
            int index;
            if (GameManager.Instance.colorState == ColorState.Painting)
            {
                // 已上过色：叠加
                if (highlightMaterial.GetInt("_Painted") == 1)
                {
                    // 生成新颜色并保存
                    Color newColor = highlightMaterial.GetColor("_PreColor") / 2 + highlightMaterial.GetColor("_Color") / 2;
                    //highlightMaterial.color = GameManager.Instance.curColor;
                    highlightMaterial.SetColor("_PreColor", highlightMaterial.GetColor("_Color"));
                    //highlightMaterial.SetColor("_Color", GameManager.Instance.curColor);
                    highlightMaterial.SetColor("_Color", newColor);

                    // 存在调色盘中但未拥有
                    if ((index = Util.FindColor(newColor)) != -1 && Globals.Instance.ownColorArr[index] != 1)
                    {
                        // 保存
                        Util.SaveColor(index);

                        // 更新调色盘ui
                        ColorUIManager.Instance.ShowColor(index, true);
                    }
                    else if (index == -1)
                    {
                        // 只显示文本提示
                        ColorUIManager.Instance.ShowColor(index, false);
                    }

                    Debug.Log(newColor + " " + Color.gray + " " + (Color.red / 2 + Color.green / 2) + " " + Color.yellow);
                }
                else
                {
                    highlightMaterial.SetColor("_PreColor", highlightMaterial.GetColor("_Color"));
                    highlightMaterial.SetInt("_Painted", 1);
                    highlightMaterial.SetColor("_Color", GameManager.Instance.curColor);
                }

                // 是否触发动画
                Color cur_color = highlightMaterial.GetColor("_Color");
                if ((index = Util.FindActivatedTag(this.gameObject.tag)) != -1 && Consts.activatedColorTable[index].Value == cur_color)
                {
                    if (anim != null)
                    {
                        highlightMaterial.SetInt("_Painted", 0);
                        highlightMaterial.SetInt("_ShowOutline", 0);

                        anim.SetBool("activated", true);

                        StartCoroutine(Hide());
                    }
                    else if (gameObject.tag == Consts.Left_Wind_Ground)
                    {
                        var material = transform.parent.Find("right_ground").GetComponent<HighlightController>().highlightMaterial;
                        Color right_color = material.GetInt("_Painted") == 1 ? material.GetColor("_Color") : Color.black;
                        if (right_color == Color.black)
                        {
                            Debug.Log("generate wind");
                            // 生成风
                            GetComponentInChildren<ParticleSystem>().Play();
                            AudioMgr.Instance.PlayEffect(Consts.Audio_Wind);
                            //anim = GetComponentInChildren<Animator>();
                            //anim.SetBool("activated", true);

                            // 船动
                            Transform boat = transform.parent.Find("boat");
                            if (boat)
                            {
                                boat.GetComponent<Boat>().isMoving = true;
                            }

                            // 火焰粒子&boss死亡判定
                            Transform fire = transform.parent.Find("fire");
                            if (fire)
                            {
                                //Color color = fire.GetComponent<HighlightController>().highlightMaterial.GetInt("_Painted") == 1 ? fire.GetComponent<HighlightController>().highlightMaterial.GetColor("_Color") : Color.black;
                                Color color = fire.GetComponent<HighlightController>().highlightMaterial.GetColor("_Color");
                                Debug.Log("fire color" + color);
                                if (color == Color.red)
                                {
                                    fire.GetComponentInChildren<ParticleSystem>().Play();
                                    AudioMgr.Instance.PlayEffect(Consts.Audio_Fire);

                                    //StartCoroutine(GrassBossDie());
                                    GameObject.FindGameObjectWithTag(Consts.Boss).GetComponent<Boss>().GrassBossDie();
                                }
                            }
                        }
                    }
#if false
                    else if (gameObject.tag == Consts.Right_Wind_Ground)
                    {
                        var material = transform.parent.Find("left_ground").GetComponent<HighlightController>().highlightMaterial;
                        Color left_color = material.GetInt("_Painted") == 1 ? material.GetColor("_Color") : Color.black;
                        if (left_color == Color.black)
                        {
                            Debug.Log("generate left wind");
                            // 生成风
                            GetComponentInChildren<ParticleSystem>().Play();
                            AudioMgr.Instance.PlayEffect(Consts.Audio_Wind);
                            //anim = GetComponentInChildren<Animator>();
                            //anim.SetBool("activated", true);

                            // 船动
                            //Transform boat = transform.parent.Find("boat");
                            //if (boat)
                            //{
                            //    boat.GetComponent<Boat>().isMoving = true;
                            //}

                            //// 火焰粒子&boss死亡判定
                            //Transform fire = transform.parent.Find("fire");
                            //if (fire)
                            //{
                            //    //Color color = fire.GetComponent<HighlightController>().highlightMaterial.GetInt("_Painted") == 1 ? fire.GetComponent<HighlightController>().highlightMaterial.GetColor("_Color") : Color.black;
                            //    Color color = fire.GetComponent<HighlightController>().highlightMaterial.GetColor("_Color");
                            //    Debug.Log("fire color" + color);
                            //    if (color == Color.red)
                            //    {
                            //        fire.GetComponentInChildren<ParticleSystem>().Play();
                            //        AudioMgr.Instance.PlayEffect(Consts.Audio_Fire);

                            //        //StartCoroutine(GrassBossDie());
                            //        GameObject.FindGameObjectWithTag(Consts.Boss).GetComponent<Boss>().GrassBossDie();
                            //    }
                            //}
                        }
                    }
#endif
                    else if (gameObject.tag == Consts.Circle_b || gameObject.tag == Consts.Circle_g || gameObject.tag == Consts.Circle_r)
                    {
                        // TODO add container script
                        if (gameObject.tag == Consts.Circle_r)
                            gameObject.transform.parent.GetComponent<Container>().OwnColor[0] = true;
                        else if (gameObject.tag == Consts.Circle_g)
                            gameObject.transform.parent.GetComponent<Container>().OwnColor[1] = true;
                        else
                            gameObject.transform.parent.GetComponent<Container>().OwnColor[2] = true;

                        gameObject.transform.parent.GetComponent<Container>().ActivatedCount++;
                    }else if (gameObject.tag.Split('_')[0] == Consts.Rainbow)
                    {
                         Debug.Log("rainbow");
                       // TODO rainbow
                        int id = int.Parse(gameObject.tag.Split('_')[1]);
                        gameObject.transform.parent.GetComponent<Rainbow>().OwnColor[id-1] = true;
                        gameObject.transform.parent.GetComponent<Rainbow>().ActivatedCount++;

                    }
                    //Debug.Log(anim.GetBool("activated"));
                }

                // 引导处理
                if (GuideManager.Instance!=null && gameObject.tag==Consts.Ice && GuideManager.Instance.IsGuiding)
                {
                    GuideManager.Instance.CancelHightLight_ice();
                }
            }
            else if (GameManager.Instance.colorState == ColorState.Erasing)
            {
                //highlightMaterial.color = Color.black;
                //highlightMaterial.SetColor("_Color", Color.black);
                highlightMaterial.SetInt("_ShowOutline", 0);
                highlightMaterial.SetInt("_Painted", 0);
            }
            //ho.On();
            GameManager.Instance.colorState = ColorState.None;
            highlight = false;
        }

        if (gameObject.tag == Consts.Plant)
        {
            gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        }
        //AfterUpdate();
    }

    IEnumerator Hide()
    {
        if (gameObject.tag == Consts.Green_Tree)
        {
            for (int i = 1; i <= 6; i++)
            {
                transform.localPosition += new Vector3(0, 0.2f, 0);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(3f);
        }
        else if (gameObject.tag == Consts.Cloud)
        {
            yield return new WaitForSeconds(1f);
            AudioMgr.Instance.PlayEffect(Consts.Audio_Rain);
            yield return new WaitForSeconds(3f);
        }
        else if (gameObject.tag == Consts.Ice)
        {
            yield return new WaitForSeconds(1f);
            AudioMgr.Instance.PlayEffect(Consts.Audio_Ice);
            Destroy(GetComponent<PolygonCollider2D>());
            yield return new WaitForSeconds(3f);
            //gameObject.AddComponent<PolygonCollider2D>();
        }
        else
        {
            yield return new WaitForSeconds(4f);
        }

        if (/*gameObject.tag == Consts.Green_Tree || */gameObject.tag==Consts.Plant)
        {
            Destroy(GetComponent<PolygonCollider2D>());
            gameObject.AddComponent<PolygonCollider2D>();
        }else if(gameObject.tag == Consts.Green_Tree)
        {
            Destroy(GetComponent<PolygonCollider2D>());
            //collider.enabled = true;
            child_collider.SetActive(true);
        }
        else if (gameObject.tag == Consts.Yellow_Tree || gameObject.tag == Consts.Ice)
        {
            anim.SetBool("activated", false);
            gameObject.SetActive(false);      
        }else if (gameObject.tag == Consts.Cloud)
        {
            GetComponent<BoxCollider2D>().size = new Vector2(7, 20);
            GetComponent<BoxCollider2D>().offset = new Vector2(0, -2);
        }
        else if (gameObject.tag == Consts.Fire)
        {
            Transform ice = transform.parent.Find("ice");
            GenerateWater gw = transform.parent.GetComponentInChildren<GenerateWater>();
            if (ice != null)
            {
                ice.GetComponent<Animator>().SetBool("activated", true);
                yield return new WaitForSeconds(2f);
                ice.gameObject.SetActive(false);
                gw.generate_water = true;
                //yield return new WaitForSeconds(1f);
                anim.SetBool("activated", false);
                Destroy(GetComponent<PolygonCollider2D>());////
            }
        }
    }

    //IEnumerator Win()
    //{
    //    GameManager.Instance.gameState = GameState.GameFinalWin;
    //    yield return new WaitForSeconds(2f);
    //}

    IEnumerator GrassBossDie()
    {
        yield return new WaitForSeconds(1.5f);

    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == Consts.Player)
    //    {
    //        Debug.Log("player in");
    //        //Destroy(collision.gameObject);
    //        hasPlayer = true;
    //    }
    //}

}
