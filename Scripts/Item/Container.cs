using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Container : MonoBehaviour
{
    private int activatedCount = 0;
    public int ActivatedCount
    {
        get { return activatedCount; }
        set
        {
            activatedCount = value;
            if (activatedCount >= 3 && OwnColor[0] && OwnColor[1] && OwnColor[2])
            {
                // win
                //GameManager.Instance.gameState = GameState.GameFinalWin;
                StartCoroutine(Win());
            }
        }
    }

    public bool[] OwnColor = { false, false, false };

    private bool needAnim = false;
    private float origin_x;
    private float rand_x = 0;

    IEnumerator Win()
    {
        AudioMgr.Instance.PlayEffect(Consts.Audio_Magic);
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
        if (needAnim)
        {
            if(rand_x <= 0)
                rand_x = Random.Range(0, 0.1f);
            else
                rand_x = Random.Range(-0.1f, 0);

            transform.localPosition = new Vector3(origin_x+rand_x, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
