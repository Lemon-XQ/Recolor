using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Rainbow : MonoBehaviour
{
    private int activatedCount = 0;
    public int ActivatedCount
    {
        get { return activatedCount; }
        set
        {
            activatedCount = value;
            //Debug.Log(activatedCount+" "+ OwnColor[0]+" "+ OwnColor[1]+" "+ OwnColor[2]+ " "+OwnColor[3]
                //+ OwnColor[4]+" "+ OwnColor[5]+" "+ OwnColor[6]);
            if (activatedCount >= 7 && OwnColor[0] && OwnColor[1] && OwnColor[2] && OwnColor[3]
                 && OwnColor[4] && OwnColor[5] && OwnColor[6])
            {
                //Debug.Log("finish collect");
                // anim
                needAnim = true;
                //GameManager.Instance.gameState = GameState.GameFinalWin;
                //StartCoroutine(Win());
            }
        }
    }

    public bool[] OwnColor = { false, false, false, false, false, false, false };

    private bool needAnim = false;
    private float origin_x;
    private float rand_x = 0;

    public Transform pivot;

    IEnumerator Win()
    {
        needAnim = true;
        yield return new WaitForSeconds(2f);
        GameManager.Instance.gameState = GameState.GameFinalWin;
        needAnim = false;
    }

    void Start()
    {
        origin_x = transform.localPosition.x;
    }

    void Update()
    {
        if (needAnim && (transform.rotation.eulerAngles.z-90)<0.1f)
        {
            transform.RotateAround(pivot.localPosition, new Vector3(0, 0, 1), -2);
            Debug.Log(transform.rotation.eulerAngles);
            //if (rand_x <= 0)
            //    rand_x = Random.Range(0, 0.1f);
            //else
            //    rand_x = Random.Range(-0.1f, 0);

            //transform.localPosition = new Vector3(origin_x + rand_x, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
