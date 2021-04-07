using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour
{
    public bool isMoving = false;
    public bool hasPlayer = false;
    public float target_x;

    private Rigidbody2D rigidbody;
    private Rigidbody2D rigidbody_player;
    public bool firstIn = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Transform player = GameObject.FindGameObjectWithTag(Consts.Player).transform;
        rigidbody_player = player.GetComponent<Rigidbody2D>();
   }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            //rigidbody.bodyType = RigidbodyType2D.Static;
            ////rigidbody = gameObject.AddComponent<Rigidbody2D>();
            ////rigidbody.mass = 1e-10f;

            //Debug.Log(boat.localPosition);
            float step = Consts.BoatMoveSpeed * Time.deltaTime;
            //float target_x = 38;
            transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, target_x, step), transform.localPosition.y, transform.localPosition.z);

            if (hasPlayer)
            {
                Transform player = GameObject.FindGameObjectWithTag(Consts.Player).transform;
                //player.localPosition = new Vector3(Mathf.Lerp(player.localPosition.x, target_x, step), player.localPosition.y, player.localPosition.z);
                Debug.Log(player.localPosition + " " + transform.localPosition);
                player.localPosition = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
            }
            if (Mathf.Abs(transform.localPosition.x - target_x) < 0.1)
            {
                isMoving = false;
                hasPlayer = false;
            }
        }else
        {
            //rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == Consts.Player)
    //    {
    //        Debug.Log("player in");
    //        //Destroy(collision.gameObject);
    //        hasPlayer = true;

    //        Destroy(rigidbody);// 不然人会推动船
    //        //rigidbody.bodyType = RigidbodyType2D.Static;
    //        //rigidbody_player.bodyType = RigidbodyType2D.Static;
    //    }
    //}

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Consts.Player)
        {
            Debug.Log("player in");

            hasPlayer = true;
            if (firstIn)
            {
                //Destroy(collision.gameObject);
                Destroy(rigidbody);// 不然人会推动船

                firstIn = false;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Consts.Player)
        {
            Debug.Log("player out");
            //Destroy(collision.gameObject);

            hasPlayer = false;
            //rigidbody_player.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
