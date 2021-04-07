using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ResetObject : MonoBehaviour
{
    //public UnityEvent OnResetHandler;

    private Vector3 defaultPos;

    void Start()
    {
        defaultPos = transform.localPosition;
    }

    public void CustomReset()
    {
        transform.localPosition = defaultPos;

        if (gameObject.GetComponent<HighlightController>() != null)
        {
            if (gameObject.tag != Consts.Green_Tree)
            {
                Destroy(gameObject.GetComponent<HighlightController>());
                gameObject.AddComponent<HighlightController>();
            }

            if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().SetBool("activated", false);
            }

            if (GetComponent<ParticleSystem>() != null)
            {
                GetComponent<ParticleSystem>().Stop();
            }

            if (GetComponentInChildren<ParticleSystem>() != null)
            {
                GetComponentInChildren<ParticleSystem>().Stop();
            }
        }

        // TODO 复原（animator set bool?)
        if (gameObject.tag == Consts.Water)
        {
            gameObject.GetComponent<PolygonCollider2D>().isTrigger = false;
            transform.parent.Find("bottom_ground").GetComponent<GenerateWater>().generate_water = false;
            transform.parent.Find("bottom_ground").GetComponent<GenerateWater>().firstTrigger = true;
        }
        else if (gameObject.tag == Consts.Boat)
        {
            gameObject.GetComponent<Boat>().hasPlayer = false;
            gameObject.GetComponent<Boat>().firstIn = true;
            gameObject.GetComponent<Boat>().isMoving = false;
            if (gameObject.GetComponent<Rigidbody2D>() == null)
                gameObject.AddComponent<Rigidbody2D>().freezeRotation = true;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else if (gameObject.tag == Consts.Cloud)
        {
            GetComponent<BoxCollider2D>().size = new Vector2(7.08f, 7.469448f);
            GetComponent<BoxCollider2D>().offset = new Vector2(0, 1.765276f);            
            GetComponent<BoxCollider2D>().isTrigger = true;            
            //Destroy(gameObject.GetComponent<BoxCollider>());
            //gameObject.AddComponent<BoxCollider>();
        }
        ////else if (gameObject.tag == Consts.Green_Tree)
        ////{
        ////    if (GetComponent<PolygonCollider2D>() == null)
        ////    {
        ////        gameObject.AddComponent<PolygonCollider2D>();
        ////    }
        ////    gameObject.GetComponent<HighlightController>().child_collider.SetActive(false);
        ////    gameObject.GetComponent<HighlightController>().highlightMaterial = new Material(Shader.Find("Custom/Highlight"));
        ////}
        //else if (gameObject.tag == Consts.Left_Wind_Ground)
        //{
        //    GetComponentInChildren<ParticleSystem>().Stop();

        //}

        //if (gameObject.name == "DynamicBricks")
        //else if (gameObject.name == "Bomb")
        //{
        //    transform.localPosition = FlyBomb.DefaultPos;
        //    Vector3 rotation = transform.localEulerAngles;
        //    rotation.z = 0; 
        //    transform.localEulerAngles = rotation;

        //}

        //OnResetHandler.Invoke();
    }
}
