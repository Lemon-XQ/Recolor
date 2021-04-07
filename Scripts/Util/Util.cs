using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class Util
{
    static StreamWriter writer;
    static StreamReader reader;

    // 返回颜色索引
    public static int FindColor(Color target)
    {
        for(int i = 0; i < Consts.ColorNum; i++)
        {
            if (Consts.colorTable[i] == target)
                return i;
        }
        return -1;
    }

    // 返回键值对索引
    public static int FindActivatedTag(string tag)
    {
        for(int i = 0; i < Consts.ActivatedColorNum; i++)
        {
            if (Consts.activatedColorTable[i].Key == tag)
                return i;
        }
        return -1;
    }

    // 修改颜色状态为已拥有
    public static void SaveColor(int index)
    {
        Globals.Instance.ownColorArr[index] = 1;
        Globals.Instance.ownColorNum++;

        StringBuilder code = new StringBuilder(PlayerPrefs.GetString("OwnColor"));
        code[index] = '1';
        PlayerPrefs.SetString("OwnColor", code.ToString());
    }

    // 把所有的数据写入文本中
    public static void WriteIntoTxt(string info)
    {
        FileInfo file = new FileInfo(Application.dataPath + "/scores.txt");
        if (!file.Exists)
        {
            writer = file.CreateText();
        }
        else
        {
            writer = file.AppendText();
        }
        writer.WriteLine(info);
        writer.Flush();
        writer.Dispose();
        writer.Close();
    }

    // 读取分数 存储到列表中（同个玩家只取最高分）
    public static List<Dictionary<string, int>> ReadScores()
    {
        Dictionary<string, int> scores_normal_mode = new Dictionary<string, int>();
        Dictionary<string, int> scores_inf_mode = new Dictionary<string, int>();

        reader = new StreamReader(Application.dataPath + "/scores.txt", Encoding.UTF8);
        string text;
        while ((text = reader.ReadLine()) != null)
        {
            string name = text.Split(' ')[0];
            int mode = int.Parse(text.Split(' ')[1]);
            int score = int.Parse(text.Split(' ')[2]);
            //Debug.Log(name + " " + mode.ToString() + " "+score.ToString());
            if (mode == 0)
            {
                if (scores_normal_mode.ContainsKey(name))
                {
                    if (scores_normal_mode[name] < score) scores_normal_mode[name] = score;
                }
                else
                    scores_normal_mode.Add(name,score);
            }else
            {
                if (scores_inf_mode.ContainsKey(name))
                {
                    if (scores_inf_mode[name] < score) scores_inf_mode[name] = score;
                }
                else
                    scores_inf_mode.Add(name, score);
            }
        }
        reader.Dispose();
        reader.Close();

        List<Dictionary<string, int>> _list = new List<Dictionary<string, int>>();
        _list.Add(scores_normal_mode);
        _list.Add(scores_inf_mode);
        return _list;
    }

    // 生成随机不重复数字的数组
    public static int[] GetRandomSequence(int total, int n, int start=0,int interval=0)
    {
        // 随机总数组
        int[] sequence = new int[total];
        // 取到的不重复数字的数组长度
        int[] output = new int[n];
        int index = 0;
        for (int i = start; i < total+start; i++)
        {
            sequence[index++] = i + interval;
        }
        int end = total - 1;
        for (int i = 0; i < n; i++)
        {
            // 随机一个数，每随机一次，随机区间-1
            int num = Random.Range(0, end + 1);
            output[i] = sequence[num];
            // 将区间最后一个数赋值到取到数上
            sequence[num] = sequence[end];
            end--;
        }
        return output;
    }
}

