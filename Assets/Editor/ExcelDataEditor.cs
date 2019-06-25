using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class ExcelDataEditor : MonoBehaviour
{
    private const string path = "/TabToy/";
    private const string globalPath = "GlobalExcel";
    private const string fileName1 = "Test1.bat";
    private const string fileName2 = "Test2.bat";
    private const string fileName3 = "UpdateSVN.bat";

    [MenuItem("数据/一键导入")]
    public static void OneStep()
    { 
        //UpdateSVN();
        CopyExcel(false);
        UpdateClass(true); 
        UpdateSerializable(); 
        UpdateData();
        //Localization.UpdatedConfig();
    }

    static void UpdateSVN()
    {
        var p1 = Application.dataPath + path + fileName3; 
        string paths = GetSVNPath(); 
        string cmd=@"/c svn checkout "+paths;
        System.Diagnostics.Process newProcess = System.Diagnostics.Process.Start("TortoiseProc.exe", @"/command:update /path:" + paths+ @" /notempfile /closeonend:1");// System.Diagnostics.Process.Start("CMD.exe",cmd); 
        newProcess.WaitForExit();
    }

    [MenuItem("数据/从SVN目录拷贝Excel")]
    public static void CopyExcel()
    {
        CopyExcel(true);
    }
    
    static string GetSVNRootPath()
    {
        string paths = Application.dataPath + "/../../../svnExcelPath.txt";
        if (File.Exists(paths))
        {
            paths = File.ReadAllText(paths);
        }
        else
        {
            paths = File.ReadAllText(Application.dataPath + path + "svnExcelPath.txt");
        }
        paths = paths + "/../../";
        return paths;
    }

    static string GetSVNPath()
    {
        string paths = Application.dataPath + "/../svnExcelPath.txt";
        if (File.Exists(paths))
        {
            paths = File.ReadAllText(paths);
        }
        else
        {
            paths = File.ReadAllText(Application.dataPath + path + "svnExcelPath.txt");
        }
        return paths;
    }
    static void CopyExcel(bool alert)//拷贝到项目文件夹下
    {
        string paths = GetSVNPath(); 
        CopyExcel(paths, Application.dataPath + path + "Excel");
        if(alert)
            EditorUtility.DisplayDialog("拷贝成功！", "Excel 拷贝成功！", "ok"); 
    }
    

    [MenuItem("数据/更新数据类")]
    public static void UpdateClass()
    {
        UpdateClass(false);
    }
    static void UpdateClass(bool waitProcess=false)//更新数据类到热更层
    {
        var p1 = Application.dataPath + path + fileName2;
        System.Diagnostics.Process newProcess = System.Diagnostics.Process.Start(p1); 
        if(waitProcess)
            newProcess.WaitForExit();
    }

    [MenuItem("数据/更新数据")]
    public static void UpdateData()//更新数据到bundle层
    {
        var p = Application.dataPath + path + fileName1;
        Application.OpenURL(p);
    }
    
   

    
    private static void LogErr(params object[] infos)
    {
        var info = "";
        for (int i = 0; i < infos.Length; i++)
        {
            info += (" " + infos[i].ToString());
        }
        Debug.LogError(info);
    }

    public class ReplaceString
    {
        public string source;
        public string target;
        public ReplaceString(string source,string target)
        {
            this.source = source;
            this.target = target;
        }
    }
    [MenuItem("数据/生成修改后的数据类")]
    public static void UpdateSerializable()
    {
        ReplaceString[] replaceStrings = new ReplaceString[] {
           new ReplaceString("namespace table", "namespace Company.Cfg")
        };

        GenerateCode(replaceStrings , "../../Assets/ExcelBin/ConfigFixed.cs");
    }
    static void GenerateCode(ReplaceString[] strNameSpace,string strPath)
    {
        var p = Application.dataPath + path + "../../Hotfix/TabToy/Bin/Config.cs";
        string text = string.Empty;
        using (FileStream fs = new FileStream(p, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs))
            {

                text = sr.ReadToEnd(); 

                foreach (ReplaceString item in strNameSpace)
                {
                    text = text.Replace(item.source,item.target);
                }
                
                text = text.Replace("// Defined in table: Globals", "// Defined in table: Globals\n[System.Serializable]");
                text = text.Replace(
                    "DO NOT EDIT!!",
                    "DO NOT EDIT!! 这个是自动生成的数据类");
                text = text.Replace(
                    "public partial class Config",
                    "[System.Serializable]public partial class Config");
                //替换枚举，直接强转
                string[] codeTexts = text.Split('\n');
                List<string> enumTypeStringList = new List<string>();
                for (int idx = 0; idx < codeTexts.Length; idx++)
                {
                    if (codeTexts[idx].Contains("reader.ReadEnum<"))
                    {
                        string tempString = codeTexts[idx];
                        int beginIdx = codeTexts[idx].IndexOf("<");
                        int endIdx = codeTexts[idx].IndexOf(">");
                        string enumTypeString = tempString.Substring(beginIdx + 1, endIdx - 1 - beginIdx);
                        enumTypeStringList.Add(enumTypeString);
                    }
                }
                for (int enumStringIdx = 0; enumStringIdx < enumTypeStringList.Count; enumStringIdx++)
                {
                    string oldValue = "reader.ReadEnum<" + enumTypeStringList[enumStringIdx] + ">();";
                    string newValue = "(" + enumTypeStringList[enumStringIdx] + ")" + "reader.ReadInt32();";
                    text = text.Replace(oldValue, newValue);
                }

            }

        }
        p = Application.dataPath + path + strPath;

        using (FileStream fs = new FileStream(p, FileMode.Create, FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(text);
            }
        }
    }

    static void CopyExcel(string sourcePath,string targetPath)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(sourcePath);
        CopyExcel(dirInfo, targetPath);
    }
    static void CopyExcel(DirectoryInfo dirInfo,string targetPath)
    {
        foreach (var item in dirInfo.GetDirectories())
        {
            CopyExcel(item, targetPath);
        }
        foreach (var item in dirInfo.GetFiles())
        {
            if (item.Extension.Trim() == ".xlsx" && item.Name.Contains("Globals"))
            {
                File.Copy(item.FullName, Application.dataPath + path + globalPath + "/" + item.Name, true);
            }
            else if (item.Extension.Trim()== ".xlsx")
            {
                File.Copy(item.FullName, targetPath + "/" + item.Name,true);
            }
        }
    }
    
}
