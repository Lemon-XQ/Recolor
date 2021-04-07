using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class Globals : UnitySingleton<Globals>
{
    //public string username { get; set; }
    //public int chapter { get; set; }
    //public int mode { get; set; } // 游戏模式
    //public bool signIn = false;
    public int curLevel { get; set; } // 关卡
    public int passLevel { get; set; } // 关卡
    public bool hasLoadIntro = false;
    //public bool hasGetR = false;
    //public bool hasGetG = false;
    //public bool hasGetB = false;
    public bool hasFinishGuide = false;
    public int[] ownColorArr;// = new int[Consts.ColorNum];
    public int ownColorNum = 0;
    public Vector2 startPos;

    public override void Awake()
    {
        base.Awake();

        ownColorArr = new int[Consts.ColorNum];
        curLevel = 0;
        passLevel = 0;

        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.SetInt("Level", 3);
        //PlayerPrefs.SetInt("Has_LoadIntro",1);
        //PlayerPrefs.SetInt("Has_FinishGuide", 1);
        if (PlayerPrefs.HasKey("Level"))
            passLevel = PlayerPrefs.GetInt("Level");


#if false
        PlayerPrefs.DeleteKey("Has_LoadIntro");
        PlayerPrefs.DeleteKey("Has_FinishGuide");
        PlayerPrefs.DeleteKey("StartPos_X");
        PlayerPrefs.DeleteKey("StartPos_Y");
        PlayerPrefs.DeleteKey("OwnColor");
        PlayerPrefs.DeleteKey("Level");
        PlayerPrefs.SetInt("Level", 3);
        PlayerPrefs.SetInt("Has_LoadIntro", 1);
        PlayerPrefs.SetInt("Has_FinishGuide", 1);
#endif

        if (PlayerPrefs.HasKey("Has_LoadIntro"))
            hasLoadIntro = PlayerPrefs.GetInt("Has_LoadIntro") > 0;
        //if (PlayerPrefs.HasKey("Has_Get_R"))
        //    hasGetR = PlayerPrefs.GetInt("Has_Get_R") > 0;
        //if (PlayerPrefs.HasKey("Has_Get_G"))
        //    hasGetG = PlayerPrefs.GetInt("Has_Get_G") > 0;
        //if (PlayerPrefs.HasKey("Has_Get_B"))
        //    hasGetB = PlayerPrefs.GetInt("Has_Get_B") > 0;
        if (PlayerPrefs.HasKey("Has_FinishGuide"))
            hasFinishGuide = PlayerPrefs.GetInt("Has_FinishGuide") > 0;

        if (PlayerPrefs.HasKey("OwnColor"))
        {
            string code = PlayerPrefs.GetString("OwnColor");
            for (int i = 0; i < Consts.ColorNum; i++)
            {
                ownColorArr[i] = int.Parse(code[i].ToString());
                if (ownColorArr[i] == 1) ownColorNum++;
            }
        }
        else
        {
#if false //debug
            StringBuilder code = new StringBuilder(new string('1', Consts.ColorNum));
            Debug.Log(code + " " + code.Length+" "+ Consts.ColorNum+" "+ownColorArr.Length);
            for (int i = 0; i < Consts.ColorNum; i++)
            {
                ownColorArr[i] = 1;
            }
            ownColorNum = Consts.ColorNum;
            
            //PlayerPrefs.SetString("OwnColor", code.ToString());

            //PlayerPrefs.DeleteKey("StartPos_X");
            //PlayerPrefs.DeleteKey("StartPos_Y");

#else
            StringBuilder code = new StringBuilder(new string('0', Consts.ColorNum));
            for (int i = 0; i < Consts.ColorNum; i++)
            {
                ownColorArr[i] = 0;
            }
            // 默认初始有黑白两色
            code[0] = '1';   code[1] = '1';
            ownColorArr[0] = 1; ownColorArr[1] = 1;
            ownColorNum = 2;
            PlayerPrefs.SetString("OwnColor", code.ToString());
#endif

            Debug.Log(code.ToString());
        }

        //if (PlayerPrefs.HasKey("StartPos_X") && PlayerPrefs.HasKey("StartPos_Y"))
        //{
        //    startPos.x = PlayerPrefs.GetFloat("StartPos_X");
        //    startPos.y = PlayerPrefs.GetFloat("StartPos_Y");
        //}else
        //{
        //    startPos = new Vector2(0, 0); // init
        //}
    }
}
