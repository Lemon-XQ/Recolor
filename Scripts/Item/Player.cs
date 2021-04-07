using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private RectTransform recttransform;
    private BoxCollider2D collider;
    private PlayerAnimation anim;
    private bool dir_left = false;

    [Header("保存文本")]
    public Text saveText;

    [Header("跳跃监测点")]
    public Transform CheckPoint;
    [Header("跳跃监测半径")]
    public float CheckRadius = 1.0f;
    [Header("跳跃监测层")]
    public LayerMask WhatIsGround;

    // 角色是否着地--true
    public bool isGround;

    public bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
        set
        {
            if (isDead != value)//如果死亡状态与当前一致则不用修改
            {
                isDead = value;
                if (value)
                {
                    GameManager.Instance.gameState = GameState.GameOver;
                    //UIManager.Instance.PushUIPanel("UILose");
                    //AudioMgr.Instance.PlayMusic(Consts.Audio_Over);
                }
            }
        }
    }

    private float scale_;
    public  bool  isClimbing = false;
    private float gravityScale;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        recttransform = GetComponent<RectTransform>();
        collider = GetComponent<BoxCollider2D>();
        anim = GetComponent<PlayerAnimation>();

        scale_ = transform.localScale.x;
        gravityScale = rigidbody.gravityScale;
    }

    void Start()
    {
        GameManager.Instance.gameState = GameState.Start;

        // 动态初始化角色碰撞盒（自适应分辨率）
        //collider.offset = Vector2.zero;
        //collider.size = new Vector2((recttransform.rect.xMax - recttransform.rect.xMin)/2, recttransform.rect.yMax - recttransform.rect.yMin - 6.0f);

        //TODO BUG?
        Debug.Log(PlayerPrefs.HasKey("Level")+"cur level" +Globals.Instance.curLevel);
        // 读取保存点
        if (PlayerPrefs.HasKey("Level") && PlayerPrefs.GetInt("Level") == Globals.Instance.curLevel // 只保存当前关卡进度
            && PlayerPrefs.HasKey("StartPos_X") && PlayerPrefs.HasKey("StartPos_Y"))
        {
            Debug.Log(transform.localPosition);
            transform.localPosition = new Vector2(PlayerPrefs.GetFloat("StartPos_X"), PlayerPrefs.GetFloat("StartPos_Y"));
            Globals.Instance.startPos = transform.localPosition;
        }else
        {
            PlayerPrefs.SetFloat("StartPos_X", transform.localPosition.x);
            PlayerPrefs.SetFloat("StartPos_Y", transform.localPosition.y);
            Globals.Instance.startPos = transform.localPosition;
        }
    }

    void Update()
    {
        isGround = Physics2D.OverlapCircle(CheckPoint.position, CheckRadius, WhatIsGround);

        if (rigidbody == null || GameManager.Instance.gameState != GameState.Start) return;

        if (IsDead)
        {
            anim.DieState();
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            return;
        }

        if (transform.localPosition.y < -6)
        {
            IsDead = true;
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (!h.Equals(0)/*Mathf.Abs(h - 0) > 0.01f*/)
        {
            // 贴图方向
            transform.localScale = new Vector3(dir_left ? /*-transform.localScale.x*/-scale_ :
                /*transform.localScale.x*/scale_, transform.localScale.y, transform.localScale.z); //new Vector3(0, h < 0 ? 0 : -180, 0);

            rigidbody.velocity = new Vector2(h * Consts.MoveSpeed, rigidbody.velocity.y);

            if (isGround)
                anim.RunState(2);
            //anim.RunState(h < 0 ? 1 : 2);

            if (h < 0) dir_left = true;
            else dir_left = false;
        }
        else if (!isClimbing || (isClimbing && isGround))
        {
            Debug.Log("idle");
            //if ( isGround && isClimbing)
                anim.IdleState();
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            //// 贴图方向
            //transform.localScale = new Vector3(dir_left ? /*-transform.localScale.x*/-scale_ : 
            //    /*transform.localScale.x*/scale_, transform.localScale.y, transform.localScale.z); //new Vector3(0, h < 0 ? 0 : -180, 0);
        }

        if (isClimbing)
        {
            if (!v.Equals(0)/*Input.GetKeyDown(KeyCode.UpArrow)*//* && isClimbing*/)
           {
                Debug.Log("climb");
                anim.ClimbState(true);
                //rigidbody.velocity = new Vector2(rigidbody.velocity.x, v*Consts.ClimbSpeed);

                rigidbody.gravityScale = 0;
                float step = v * Time.deltaTime * Consts.ClimbSpeed;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + step, transform.localPosition.z);
            }
        }else
        {
            //Debug.Log("climb");
            anim.ClimbState(false);
            rigidbody.gravityScale = gravityScale;
        }

        if (isGround && !isClimbing)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            //if (rigidbody.velocity.y.Equals(0))
            {
                //if (isClimbing)
                //{
                //    anim.ClimbState(true);
                //    rigidbody.velocity = new Vector2(rigidbody.velocity.x, Consts.ClimbSpeed);
                //}
                //else
                {
                    Debug.Log("jump");
                    //anim.ClimbState(false);
                    anim.JumpState(true/*, h < 0*/);
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, Consts.JumpSpeed);
                    anim.IdleState();
                }
            }else
            {
                anim.JumpState(false/*, h < 0*/);
            }
        }
        else
        {
            //if (Input.GetKeyDown(KeyCode.UpArrow))
                //anim.JumpState(false/*, h < 0*/);

            //anim.JumpState(false/*, h < 0*/);
            //anim.JumpState(false/*, false*/);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("trigger");
        if (other.tag == Consts.Save_Flag/* && GameManager.Instance.gameState == GameState.Start*/)
        {
            StartCoroutine(Save());
            //PlayerPrefs.SetFloat("StartPos_X", transform.localPosition.x);
            //PlayerPrefs.SetFloat("StartPos_Y", transform.localPosition.y);
            //Globals.Instance.startPos = transform.localPosition;
        }else if (other.tag == Consts.Ladder)
        {
            Debug.Log("ladder in");
            isClimbing = true;
        }else if (other.tag == Consts.ColorBall)
        {
            Debug.Log("win");
            AudioMgr.Instance.PlayEffect(Consts.Audio_ColorBall);

            int index = -1;
            if (Globals.Instance.curLevel == 0)
            {
                index = Util.FindColor(Color.red);
            }else if (Globals.Instance.curLevel == 1)
            {
                index = Util.FindColor(Color.green);
            }else if (Globals.Instance.curLevel == 2)
            {
                index = Util.FindColor(Color.blue);
            }
            Debug.Log(index);
            // 保存
            Util.SaveColor(index);

            GameManager.Instance.gameState = GameState.GameWin;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("trigger");
        if (other.tag == Consts.Ladder)
        {
            Debug.Log("ladder out");
            isClimbing = false;
        }
    }

    IEnumerator Save()
    {
        saveText.text = "保存中...";
        PlayerPrefs.SetFloat("StartPos_X", transform.localPosition.x + 1.0f);
        PlayerPrefs.SetFloat("StartPos_Y", transform.localPosition.y);
        Globals.Instance.startPos = new Vector3(transform.localPosition.x + 1.0f, transform.localPosition.y, transform.localPosition.z);
        yield return new WaitForSeconds(1f);

        saveText.text = "保存成功";
        yield return new WaitForSeconds(0.5f);
        saveText.text = "";
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Consts.Trap || collision.gameObject.tag == Consts.Plant/* && GameManager.Instance.gameState == GameState.Start*/)
        {
            Debug.Log("player in trap");
            IsDead = true;

            //AudioMgr.Instance.PlayMusic(Consts.Audio_Over);
            //anim.DieState();
            //StartCoroutine(Die());
            //Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == Consts.Water)
        {
            GameObject boat = collision.gameObject.transform.parent.Find("boat").gameObject;
            if (boat && boat.GetComponent<Boat>().hasPlayer) return;

            Debug.Log("player in water");
            collision.collider.isTrigger = true;
            AudioMgr.Instance.PlayEffect(Consts.Audio_InWater);
            //this.GetComponent<CapsuleCollider2D>().isTrigger = true;
        }else if (collision.gameObject.tag == Consts.Boat)
        {
            Debug.Log("in boat");
            collision.gameObject.GetComponent<Boat>().hasPlayer = true;
            //if (collision.gameObject.GetComponent<Boat>().isMoving)
            //{
            //    transform.localPosition = collision.transform.localPosition;
            //}
        }
    }

    IEnumerator Die()
    {
        IsDead = true;
        yield return new WaitForSeconds(2f);
        Debug.Log(IsDead);
        GameManager.Instance.gameState = GameState.GameOver;
    }
}
