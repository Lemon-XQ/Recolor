using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWater : MonoBehaviour
{
    public Transform water;

    public bool generate_water = false;
    public float finalPosY;
    public bool firstTrigger = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (generate_water)
        {
            float step = Consts.WaterLiftSpeed * Time.deltaTime;
            water.localPosition = new Vector3(water.localPosition.x, Mathf.Lerp(water.localPosition.y, finalPosY, step), water.localPosition.z);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Consts.Cloud)
        {
            Debug.Log("water trigger");
            //StartCoroutine(Save());
            //PlayerPrefs.SetFloat("StartPos_X", transform.localPosition.x);
            //PlayerPrefs.SetFloat("StartPos_Y", transform.localPosition.y);
            //Globals.Instance.startPos = transform.localPosition;

            generate_water = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == Consts.Cloud)
        {
            if (firstTrigger)
            {
                Debug.Log("water trigger stay");
                //StartCoroutine(Save());
                //PlayerPrefs.SetFloat("StartPos_X", transform.localPosition.x);
                //PlayerPrefs.SetFloat("StartPos_Y", transform.localPosition.y);
                //Globals.Instance.startPos = transform.localPosition;

                generate_water = true;
                firstTrigger = false;

                Destroy(other.GetComponent<BoxCollider2D>());
                other.gameObject.AddComponent<BoxCollider2D>();
            }
        }
    }
}
