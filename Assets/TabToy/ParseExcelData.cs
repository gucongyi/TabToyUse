using Company.Cfg;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ParseExcelData : MonoBehaviour
{
    public Config config;
    private byte[] bytes;

    /// <summary>
    /// 初始化数据
    /// 运行时改为手动调用
    /// </summary>
    public void InitData()
    {
        //var t = Time.realtimeSinceStartup;
        MemoryStream ms = new MemoryStream(bytes);
        var reader = new tabtoy.DataReader(ms, ms.Length);
        if (!reader.ReadHeader())
        {
            return;
        }

        Config.Deserialize(config, reader);
    }
    // Start is called before the first frame update
    void Start()
    {
        config = new Config();
        TextAsset textAsset=Resources.Load<TextAsset>("DataProxy/Bin/DataProxyConfig");
        bytes = textAsset.bytes;
        InitData();
        var define=config.GetAIValueByID(99008001);
        Debug.LogError(define.Desc);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
