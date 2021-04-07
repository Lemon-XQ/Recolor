using UnityEngine;
using UnityEngine.UI;

public class ColorUI : MonoBehaviour
{
    [Header("画笔按钮")]
    public Button penBtn;
    [Header("擦除按钮")]
    public Button eraseBtn;
    [Header("调色板面板")]
    public GameObject palettePanel;
    [Header("调色板管理器")]
    public ColorUIManager colorMgr;

    //private CameraBlur cameraBlur;
    private PiUI paletteMenu;

    void Awake()
    {
        // 初始化调色盘
        var pair = new ColorUIManager.NameMenuPair();
        pair.name = Consts.PaletteName;
        pair.menu = palettePanel.GetComponent<PiUI>();
        ColorUIManager.Instance.nameMenu.SetValue(pair, 0);// = new[] ColorUIManager.NameMenuPair;
    }

    void Start()
    {
        // 注册按钮事件
        penBtn.onClick.AddListener(delegate { OnPenBtnClick(); });
        eraseBtn.onClick.AddListener(delegate { OnEraseBtnClick(); });

        //cameraBlur = Camera.main.GetComponent<CameraBlur>();
        //cameraBlur.enabled = false;

        // 初始化调色盘
        //var pair = new ColorUIManager.NameMenuPair();
        //pair.name = Consts.PaletteName;
        //pair.menu = palettePanel.GetComponent<PiUI>();
        //ColorUIManager.Instance.nameMenu.SetValue(pair, 0);// = new[] ColorUIManager.NameMenuPair;
        paletteMenu = /*colorMgr*/ColorUIManager.Instance.GetPiUIOf(Consts.PaletteName);
        // 设置
        paletteMenu.equalSlices = true;
        paletteMenu.iconDistance = 0f;
        paletteMenu.syncColors = false;
        paletteMenu.openTransition = PiUI.TransitionType.Fan;
        paletteMenu.closeTransition = PiUI.TransitionType.SlideRight;
        // 颜色数据
        int ownColrNum = 0; // 已拥有颜色数
        for(int i = 0; i < Consts.ColorNum; i++)
        {
            if (Globals.Instance.ownColorArr[i] != 0) ownColrNum++;
        }
        paletteMenu.piData = new PiUI.PiData[Consts.ColorNum];//[ownColrNum];//
        for (int i = 0; i < /*ownColrNum*/Consts.ColorNum; i++)
        {
            paletteMenu.piData[i] = new PiUI.PiData();

            if (Globals.Instance.ownColorArr[i] == 0) // 未拥有
            {
                //paletteMenu.piData[i].isInteractable = false;
                //paletteMenu.piData[i].sliceLabel = "?";
                //paletteMenu.piData[i].disabledColor = Color.grey / 2;// + Consts.colorTable[i] / 2;
                //continue;
                paletteMenu.piData[i].hidden = true;
            }
            else
            {
                paletteMenu.piData[i].hidden = false;
            }
                paletteMenu.piData[i].nonHighlightedColor = Consts.colorTable[i];
                paletteMenu.piData[i].highlightedColor = Consts.colorTable[i];

            // 事件绑定
            paletteMenu.piData[i].onSlicePressed = new UnityEngine.Events.UnityEvent();
            int index = i;
            paletteMenu.piData[i].onSlicePressed.AddListener(delegate { OnColorBtnClick(index); });
            
            //paletteMenu.piData[i].hoverFunctions = true;
            //paletteMenu.piData[i].onHoverEnter = new UnityEngine.Events.UnityEvent();
            //paletteMenu.piData[i].onHoverEnter.AddListener(OnHoverEnter);
            //paletteMenu.piData[i].onHoverExit = new UnityEngine.Events.UnityEvent();
            //paletteMenu.piData[i].onHoverExit.AddListener(OnHoverExit);
        }
        /*colorMgr*/
        ColorUIManager.Instance.RegeneratePiMenu(Consts.PaletteName);
    }

    void OnPenBtnClick()
    {
        //palettePanel.SetActive(true);
        palettePanel.transform.parent.SetAsLastSibling();// 置于顶层
        // 打开/关闭调色盘
        ColorUIManager.Instance.ChangeMenuState(Consts.PaletteName, new Vector2(Screen.width / 2f, Screen.height / 2f));
        //colorMgr.ChangeMenuState(Consts.PaletteName, new Vector2(0, 0));

        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    void OnColorBtnClick(int index)
    {
        // 关闭调色盘
        ColorUIManager.Instance.ChangeMenuState(Consts.PaletteName, new Vector2(Screen.width / 2f, Screen.height / 2f));
        //colorMgr.ChangeMenuState(Consts.PaletteName, new Vector2(0, 0));

        // 着色
        // 高亮可上色物体
        // 点击上色
        GameManager.Instance.colorState = ColorState.Painting;
        GameManager.Instance.curColor = Consts.colorTable[index];
        
        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    void OnEraseBtnClick()
    {
        // 擦除颜色
        GameManager.Instance.colorState = ColorState.Erasing;

        AudioMgr.Instance.PlayEffect(Consts.Audio_Click);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnPenBtnClick();
        }
    }
}
