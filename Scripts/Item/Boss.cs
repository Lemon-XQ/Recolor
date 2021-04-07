using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject colorBall;

    private Animator anim;
    private bool isDead = false;
    private Transform player;
    //public float target_y;

    private bool firstTrigger = true;

    public enum BOSSTYPE
    {
        FIRE = 1,
        WATER = 2,
        GRASS = 3
    };
    public BOSSTYPE bosstype;

    //护盾
    public GameObject BossShield;

    private bool needAnim = false;
    public float origin_x;
    private float rand_x = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (BossShield)
        {
            if (bosstype != BOSSTYPE.WATER)
                BossShield.SetActive(false);
            else
                BossShield.SetActive(true);
        }

        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag(Consts.Player).transform;

        origin_x = transform.localPosition.x;

    }

    // Update is called once per frame
    void Update()
    {
        if (needAnim)
        {
            if (rand_x <= 0)
                rand_x = Random.Range(0, 0.1f);
            else
                rand_x = Random.Range(-0.1f, 0);

            transform.localPosition = new Vector3(origin_x + rand_x, transform.localPosition.y, transform.localPosition.z);
        }

        if (isDead)
        {
            //Debug.Log(Mathf.Abs(colorBall.transform.localPosition.x - player.localPosition.x));
            float step = Time.deltaTime * Consts.BallLiftSpeed;
            //if (Mathf.Abs(colorBall.transform.localPosition.y - target_y) > 0.1f)
            //{
            //    colorBall.transform.localPosition += new Vector3(0, step, 0);
            //}
            //else 
            if (Mathf.Abs(colorBall.transform.localPosition.x - player.localPosition.x) > 0.001f ||
                Mathf.Abs(colorBall.transform.localPosition.y - player.localPosition.y) > 0.001f)
            {
                //Debug.Log("mov");
                colorBall.transform.localPosition = new Vector3(
                    Mathf.Lerp(colorBall.transform.localPosition.x, player.transform.localPosition.x, step),
                    Mathf.Lerp(colorBall.transform.localPosition.y, player.transform.localPosition.y, step),
                    0
                    );
            }else
            {
                needAnim = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (bosstype==BOSSTYPE.FIRE&&other.tag == Consts.Water)
        { 
            Debug.Log("water trigger boss");
            StartCoroutine(FireBossDie());            
        }
        if (bosstype == BOSSTYPE.WATER && other.tag == Consts.Plant)
        {
            Debug.Log("plant trigger boss");
            StartCoroutine(WaterBossDie());
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (bosstype == BOSSTYPE.WATER && other.tag == Consts.Plant)
        {
            if (firstTrigger)
            {
                Debug.Log("plant trigger boss");
                StartCoroutine(WaterBossDie());
                firstTrigger = false;
            }
        }
    }

    void OnParticleTrigger2D()
    {
        Debug.Log("particle trigger");

        if (bosstype == BOSSTYPE.GRASS && firstTrigger)
        {
            firstTrigger = false;
            StartCoroutine(FireBossDie());
        }
    }


    public void OnParticleCollision(GameObject other)
    {
        //if (other.tag == "Player")
        {
            Debug.Log("hahaha");
        }
    }

    public void GrassBossDie()
    {
        StartCoroutine(FireBossDie());
    }

    IEnumerator FireBossDie()
    {
        //anim.SetBool("Die", true);
        AudioMgr.Instance.PlayEffect(Consts.Audio_BossDie);

        needAnim = true;
        yield return new WaitForSeconds(2f);
        colorBall.SetActive(true);
        yield return new WaitForSeconds(1f);
        isDead = true;
    }

    IEnumerator WaterBossDie()
    {
        AudioMgr.Instance.PlayEffect(Consts.Audio_shield);
        for(int i = 4; i >= 0; i--)
        {
            BossShield.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (float)0.2 * i);
            yield return new WaitForSeconds(0.1f);
        }
        AudioMgr.Instance.PlayEffect(Consts.Audio_BossDie);

        //anim.SetBool("Die", true);
        needAnim = true;
        yield return new WaitForSeconds(2f);
        colorBall.SetActive(true);
        yield return new WaitForSeconds(1f);
        isDead = true;
    }
}
