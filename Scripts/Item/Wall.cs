using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.tag == Consts.Barrage_Word)
        //{
        //    //Debug.Log("collide wall");
        //    // 碰墙后销毁这个字
        //    Destroy(other.gameObject);
        //}
        //else if (other.tag == Consts.Barrage_Good)
        //{
        //    //Debug.Log("collide wall");
        //    // 碰墙后销毁这个字
        //    Destroy(other.gameObject);
        //}
    }

}
