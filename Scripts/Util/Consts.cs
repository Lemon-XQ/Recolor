using UnityEngine;
using System.Collections.Generic;
using System.Collections;

enum DifficultyType
{
    Easy,
    Medium,
    Hard
}

public class Consts
{
    // tag
    public const string MainCamera = "MainCamera";
    public const string Player = "Player";
    public const string Wall = "Wall";
    public const string Paintable = "Paintable";
    public const string Trap = "Trap";
    public const string Save_Flag = "Save_Flag";
    public const string Yellow_Tree = "Yellow_Tree";
    public const string Green_Tree = "Green_Tree";
    public const string Ice = "Ice";
    public const string Fire = "Fire";
    public const string Cloud = "Cloud";
    public const string Plant = "Plant";
    public const string Left_Wind_Ground = "Left_Wind_Ground";
    public const string Right_Wind_Ground = "Right_Wind_Ground";
    public const string Circle_r = "Circle_r";
    public const string Circle_g = "Circle_g";
    public const string Circle_b = "Circle_b";
    public const string Rainbow = "Rainbow";
    public const string Rainbow_1 = "Rainbow_1";
    public const string Rainbow_2 = "Rainbow_2";
    public const string Rainbow_3 = "Rainbow_3";
    public const string Rainbow_4 = "Rainbow_4";
    public const string Rainbow_5 = "Rainbow_5";
    public const string Rainbow_6 = "Rainbow_6";
    public const string Rainbow_7 = "Rainbow_7";
    public const string Boat = "Boat";
    public const string Water = "Water";
    public const string Ladder = "Ladder";
    public const string ColorBall = "ColorBall";
    public const string Boss = "Boss";

    // scene
    public const int Scene_Menu = 0;
    public const int Scene_Main = 1;
    //public const int Scene_End = 2;

    // save
    public const string Music_Volume = "MusicVolume";
    public const string Effect_Volume = "EffectVolume";

    // loading
    public const string Loading_Scene = "LoadingScene";
    public const string Loading_Progress = "LoadingProgress";
    public const string Loading_Bar = "LoadingBar";
    // public const string Loading_Icon = "LoadingIcon";

    // audio
    public const string Audio_BGM = "music/bgm";
    public const string Audio_Click = "music/click";
    //public const string Audio_Win = "music/he_1";
    public const string Audio_FinalWin = "music/finalWin";
    //public const string Audio_Over = "music/Longest_Night";
    //public const string Audio_Hurt = "music/hurt";
    //public const string Audio_Get_Medicine = "music/get_medicine";
    //public const string Audio_Eat_Medicine = "music/eat_medicine";
    public const string Audio_BossDie = "music/boss_die";
    public const string Audio_HeroDie = "music/hurt";
    public const string Audio_Rain = "music/rain_medium";
    public const string Audio_Wind = "music/wind_normal1";
    public const string Audio_Type = "music/type";
    public const string Audio_Fire = "music/fire";
    public const string Audio_InWater = "music/water";
    public const string Audio_Magic = "music/magic";
    public const string Audio_ColorBall = "music/colorball";
    public const string Audio_Ice = "music/ice";
    public const string Audio_shield = "music/shield";

    // effect
    public const string Effect_Win = "effect/winEffect";
    public const string Effect_Over = "effect/overEffect";

    // other
    public const string PaletteName = "Palette";

    public static float[] camEndPos =
    {
        111,
        90,
        111,
        50.3f
    };

    // 颜色表
    public static Color[] colorTable =
    {
        Color.black,
        Color.white,
        Color.grey,
        Color.red,
        Color.green,
        Color.red/2+Color.green/2,//Color.yellow,
        Color.blue,
        Color.red/2+(Color.red/2+Color.green/2)/2,//Color.orange
        Color.blue/2+Color.green/2,//Color.cyan,
        Color.red/2+Color.blue/2,//Color.magenta
    };


    // 触发动画颜色表
    public static KeyValuePair<string, Color>[] activatedColorTable =
    {
        new KeyValuePair<string, Color>(Yellow_Tree, Color.red/2+Color.green/2),
        new KeyValuePair<string, Color>(Green_Tree, Color.green),
        new KeyValuePair<string, Color>(Plant, Color.green),
        new KeyValuePair<string, Color>(Ice, Color.black),
        new KeyValuePair<string, Color>(Fire, Color.red),
        new KeyValuePair<string, Color>(Cloud, Color.grey),
        new KeyValuePair<string, Color>(Left_Wind_Ground, Color.white),
        new KeyValuePair<string, Color>(Circle_r, Color.red),
        new KeyValuePair<string, Color>(Circle_g, Color.green),
        new KeyValuePair<string, Color>(Circle_b, Color.blue),
        new KeyValuePair<string, Color>(Rainbow_1, Color.red),
        new KeyValuePair<string, Color>(Rainbow_2, Color.red/2+(Color.red/2+Color.green/2)/2),//Color.orange
        new KeyValuePair<string, Color>(Rainbow_3, Color.red/2+Color.green/2),
        new KeyValuePair<string, Color>(Rainbow_4, Color.green),
        new KeyValuePair<string, Color>(Rainbow_5, Color.blue/2+Color.green/2),
        new KeyValuePair<string, Color>(Rainbow_6, Color.blue),
        new KeyValuePair<string, Color>(Rainbow_7, Color.red/2+Color.blue/2),
    };

    // 游戏数值
    public const int LevelNum = 4;
    public const int ColorNum = 10;
    public const int ActivatedColorNum = 17;

    public const float MoveSpeed = 4.0f;//150.0f; // 移动速度    
    public const float JumpSpeed = 4.0f;//300.0f; // 跳跃速度
    public const float ClimbSpeed = 3.0f;//300.0f; // 攀爬速度
    public const float JumpPower = 300.0f; // 跳跃力

    public const float HUDSpeed = 20.0f; // 特效文本上升速度
    public const float ColorChangeSpeed = 0.4f; // 渐变速度
    public const float RainHeavySpeed = 0.05f; // 雨变大速度
    public const float RainStopSpeed = 0.3f; // 雨停速度
    public const float WaterLiftSpeed = 0.5f; // 水面上升速度
    public const float BoatMoveSpeed = 0.5f; 
    public const float BallLiftSpeed = 0.8f; 
}
