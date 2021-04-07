using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum GuideEventType
{
    ClickCancel,
    ClickChildCancel,
    ClickIceCancel,
    AutoCancel,
    LeftClickCancel,
    RightClickCancel,
    EscClickCancel,
    ClickAwake,
    ClickFinish,
    None
}

[System.Serializable]
public class GuideData
{
    public GameObject guideItem;// 引导UI项
    public GuideEventType guideType;// 引导事件
}

public class GuideManager : GameSingleton<GuideManager>
{
    [Header("引导遮罩")]
    public GameObject guideMask;
    [Header("引导高亮UI项")]
    public GuideData[] guideDataArray;
    [Header("引导提示信息")]
    public GameObject[] hintInfoObjects;
    
    private GameObject currentGuideGo;
    private int nowIndex;
    private bool isGuiding = false;

    public bool IsGuiding
    {
        get { return isGuiding; }
    }

    public void BeginGuide()
    {
        GameManager.Instance.gameState = GameState.Start;
        isGuiding = true;
        guideMask.SetActive(true);
        nowIndex = 0;
        Next();
    }

    public void EndGuide()
    {
        guideMask.SetActive(false);
        isGuiding = false;
    }

    void Next()
    {
        if (nowIndex < guideDataArray.Length)
        {
            currentGuideGo = guideDataArray[nowIndex].guideItem;
            switch (guideDataArray[nowIndex].guideType)
            {
                case GuideEventType.ClickCancel:
                    hintInfoObjects[nowIndex].SetActive(true);
                    ShowHightLight(nowIndex);
                    currentGuideGo.GetComponent<Canvas>().sortingOrder = 1;
                    break;

                case GuideEventType.ClickChildCancel:
                    hintInfoObjects[nowIndex].SetActive(true);
                    ShowHightLight_child(nowIndex);
                    break;

                case GuideEventType.ClickIceCancel:
                    guideMask.SetActive(false);
                    hintInfoObjects[nowIndex].SetActive(true);
                    ShowHightLight_ice(nowIndex);
                    break;

                case GuideEventType.AutoCancel:
                    hintInfoObjects[nowIndex].SetActive(true);
                    ShowHightLight(nowIndex);
                    StartCoroutine(AutoCancelHighlight());
                    break;

                case GuideEventType.LeftClickCancel:
                    hintInfoObjects[nowIndex].SetActive(true);
                    StartCoroutine(LeftClickHandle());
                    break;

                case GuideEventType.RightClickCancel:
                    hintInfoObjects[nowIndex].SetActive(true);
                    StartCoroutine(RightClickHandle());
                    break;

                case GuideEventType.EscClickCancel:
                    hintInfoObjects[nowIndex].SetActive(true);
                    StartCoroutine(EscClickHandle());
                    break;

                case GuideEventType.ClickAwake:
                    hintInfoObjects[nowIndex].SetActive(true);
                    ShowHightLight(nowIndex);
                    currentGuideGo.GetComponent<Canvas>().sortingOrder = 1;
                    EventTriggerListener.GetListener(currentGuideGo).onClick += CancelMask;
                    break;

                case GuideEventType.ClickFinish:
                    hintInfoObjects[nowIndex].SetActive(true);
                    currentGuideGo.SetActive(true);
                    ShowHightLight(nowIndex);
                    currentGuideGo.GetComponent<Canvas>().sortingOrder = 1;
                    //isGuiding = false;
                    break;

                case GuideEventType.None:
                    hintInfoObjects[nowIndex].SetActive(true);
                    StartCoroutine(AutoNext());
                    break;

                default:
                    break;
            }
            nowIndex++;
        }
        else
        {
            hintInfoObjects[nowIndex - 1].SetActive(false);
            //Debug.Log(nowIndex + hintInfoObjects[nowIndex - 1].name);
            PlayerPrefs.SetInt("Has_FinishGuide", 1);
            EndGuide();
        }
    }

    void ShowHightLight(int index)
    {
        //guideMask.transform.SetAsLastSibling();
        GameObject go = guideDataArray[index].guideItem;
        go.AddComponent<Canvas>().overrideSorting = true;
        go.AddComponent<GraphicRaycaster>();

        //设置监听  
        EventTriggerListener.GetListener(go).onClick += CancelHightLight;
    }

    void ShowHightLight_ice(int index)
    {
        //guideMask.transform.SetAsLastSibling();
        GameObject go = guideDataArray[index].guideItem;
        //////go.AddComponent<Canvas>().overrideSorting = true;
        //////go.AddComponent<GraphicRaycaster>();
        //////go.GetComponent<Canvas>().sortingOrder = 1;

        //设置监听  
        //EventTriggerListener.GetListener(go).onClick += CancelHightLight_ice;
    }

    void ShowHightLight_child(int index)
    {
        //guideMask.transform.SetAsLastSibling();
        GameObject go = guideDataArray[index].guideItem;
        int num = go.transform.childCount;
        for(int i = 0; i < num; i++)
        {
            GameObject child = go.transform.GetChild(i).gameObject;
            child.AddComponent<Canvas>().overrideSorting = true;
            child.AddComponent<GraphicRaycaster>();
            child.GetComponent<Canvas>().sortingOrder = 1;
            //设置监听  改为外部触发(存在其他点击事件处理)
            //Debug.Log(EventTriggerListener.GetListener(child).onClick);
            child.AddComponent<EventTriggerListener>();
            //EventTriggerListener.GetListener(child).onClick += CancelHightLight_child;
        }

    }

    public void CancelHightLight_child(GameObject go)
    {
        Debug.Log("child click");
        int num = go.transform.parent.childCount;
        for (int i = 0; i < num; i++)
        {
            GameObject child = go.transform.parent.GetChild(i).gameObject;
            Destroy(child.GetComponent<GraphicRaycaster>());
            Destroy(child.GetComponent<Canvas>());
            if (EventTriggerListener.GetListener(child).onClick == CancelHightLight_child)
                EventTriggerListener.GetListener(child).onClick -= CancelHightLight_child;
        }

        hintInfoObjects[nowIndex - 1].SetActive(false);
        Next();
    }

    //public void CancelHightLight_ice(GameObject go)
    //{
    //    Debug.Log("ice click");
    //    Destroy(go.GetComponent<GraphicRaycaster>());
    //    Destroy(go.GetComponent<Canvas>());
    //    hintInfoObjects[nowIndex - 1].SetActive(false);
    //    go.SetActive(false);
    //    guideMask.SetActive(false);
    //    if (EventTriggerListener.GetListener(go).onClick == CancelHightLight_ice)
    //        EventTriggerListener.GetListener(go).onClick -= CancelHightLight_ice;
    //    Next();
    //    //Debug.Log(nowIndex + hintInfoObjects[nowIndex - 1].name);
    //    //hintInfoObjects[nowIndex - 1].SetActive(false);
    //    //EndGuide();
    //}

    public void CancelHightLight_ice()
    {
        StartCoroutine(AutoCancelHighlight_ice());
    }

    IEnumerator AutoCancelHighlight_ice()
    {
        yield return new WaitForSeconds(1.5f);
        hintInfoObjects[nowIndex - 1].SetActive(false);
        guideMask.SetActive(false);
        Next();
    }

    void CancelHightLight(GameObject go)
    {
        Destroy(go.GetComponent<GraphicRaycaster>());
        Destroy(go.GetComponent<Canvas>());
        hintInfoObjects[nowIndex-1].SetActive(false);
        Next(); 
        if(EventTriggerListener.GetListener(go).onClick == CancelHightLight)
            EventTriggerListener.GetListener(go).onClick -= CancelHightLight;
    }
    
    void CancelMask(GameObject go)
    {
        guideMask.SetActive(false);
        EventTriggerListener.GetListener(go).onClick -= CancelMask;
    }

    IEnumerator AutoNext()
    {
        yield return new WaitForSeconds(2.0f);
        Next();
    }

    IEnumerator AutoCancelHighlight()
    {
        yield return new WaitForSeconds(2f);
        CancelHightLight(currentGuideGo);
    }

    private bool leftClick, rightClick, escClick;
    IEnumerator LeftClickHandle()
    {
        while (!leftClick)
        {
            if (Input.GetMouseButtonDown(0)) leftClick = true;
            yield return 0;
        }
        hintInfoObjects[nowIndex - 1].SetActive(false);
        Next();
    }
    
    IEnumerator RightClickHandle()
    {
        while (!rightClick)
        {
            if (Input.GetMouseButtonDown(1)) rightClick = true;
            yield return 0;
        }
        hintInfoObjects[nowIndex - 1].SetActive(false);
        Next();
    }
    
    IEnumerator EscClickHandle()
    {
        while (!escClick)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) escClick = true;
            yield return 0;
        }
        hintInfoObjects[nowIndex - 1].SetActive(false);
        Next();
        guideMask.SetActive(true);
    }
	
}
