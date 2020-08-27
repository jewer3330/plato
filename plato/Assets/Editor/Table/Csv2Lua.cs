using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using TableTool;

public class Csv2Lua 
{
    //[MenuItem("Tools/Table/CSV2Lua")]
    public static void Run(Config config)
    {
        splitor = config.SplitChar;
        outPutPath = "Assets/" + config.tableluaPath;
        string[] files = Directory.GetFiles("Assets/"+ config.sourcePath,"*.csv");
        foreach (var k in files)
        {
            string content = File.ReadAllText(k);
            string fileName = Path.GetFileNameWithoutExtension(k);
            Parse(fileName, content,config.offset);
        }
    }

    public static char splitor = ',';
    public static string outPutPath = "Assets/Lua/Table";
    /// <summary>
    /// 第一行是变量名称
    /// 第二行是类型
    /// 第三行至n行是数据
    /// </summary>
    /// <param name="content"></param>
    public static void Parse(string name,string content,int offset)
    {
        string[] lines = content.Replace("\r\n", "\n").Split(new char[] { '\n' },System.StringSplitOptions.RemoveEmptyEntries);
        string[] members = lines[offset + 0].Split(splitor);
        string[] types = lines[offset + 1].Split(splitor);

        List<string> all = new List<string>();

        for (int i = 2 + offset; i < lines.Length;i++)
        {
            string lin = string.Empty;
            string[] datas = lines[i].Split(splitor);
            string cen = string.Empty;
            for(int j = 0; j < datas.Length;j++)
            {
                string line = string.Empty;
                if(types[j] == "STRING")
                    line = luaLineFormatString.Replace("#Member", members[j]).Replace("#Data", datas[j]);
                else
                    line = luaLineFormatNum.Replace("#Member", members[j]).Replace("#Data", datas[j]);
                cen += line;
            }

            lin = luaFormat.Replace("#Key", datas[0]).Replace("#LineFormat", cen);
            all.Add(lin);
        }
        string r = string.Empty;
        foreach(var k in all)
        {
            r += k;
        }

        string ret = luaTableFormat.Replace("#Name", name).Replace("#Content", r);

        string path = string.Format("{0}/{1}.lua", outPutPath, name);

        //SuperBoBo.FileUtils.WriteFile(path,ret,false);
        System.IO.File.WriteAllText(path, ret);
    }

    public static string luaTableFormat = @"
local #Name = {#Content
    }
return #Name";

    public static string luaFormat = @"
[#Key] = {#LineFormat},";

    public static string luaLineFormatNum =@"#Member  = #Data,";
    public static string luaLineFormatString = @"#Member  = '#Data',";
}
