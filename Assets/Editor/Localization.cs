//using HYZ.Cfg.ETModel;
//using System.Collections.Generic;
//using System.IO;


//#if UNITY_EDITOR
//using UnityEditor;
//public static class Localization
//{
//    public static Config config;
//    private static byte[] bytes;
//    // Key = Values dictionary (multiple languages)
//    private static Dictionary<string, string[]> mDictionary = new Dictionary<string, string[]>();
//    private static bool localizationHasBeenSet = false;
//    public static string[] knownLanguages;
//    public static Dictionary<string, string[]> dictionary
//    {
//        get
//        {
//            if (!localizationHasBeenSet) Load();
//            return mDictionary;
//        }
//        set
//        {
//            localizationHasBeenSet = (value != null);
//            mDictionary = value;
//        }
//    }
//    private static List<string> keys = new List<string>();
//    public static bool keysHasBeenSet = false;
//    public static List<string> Keys
//    {
//        get
//        {
//            if (!keysHasBeenSet)
//            {
//                keys = new List<string>();

//                foreach (KeyValuePair<string, string[]> pair in dictionary)
//                {
//                    if (pair.Key == "KEY") continue;
//                    keys.Add(pair.Key);
//                }
//                keys.Sort(delegate (string left, string right) { return left.CompareTo(right); });
//                keysHasBeenSet = true;
//            }
//            return keys;
//        }
//    }

//    private static void Load()
//    {
//        config = new Config();
//        var realPath = AssetDatabase.GetAssetPathsFromAssetBundle("dataproxyconfig.unity3d");
//        foreach (string s in realPath)
//        {
//            string assetName = Path.GetFileNameWithoutExtension(s);
//            string path = $"{assetName}/dataproxyconfig".ToLower();
//            UnityEngine.TextAsset textAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.TextAsset>(s);
//            bytes = textAsset.bytes;
//            MemoryStream ms = new MemoryStream(bytes);
//            var reader = new tabtoy.ETModel.DataReader(ms, ms.Length);
//            if (!reader.ReadHeader())
//                return;
//            Config.Deserialize(config, reader);

//            System.Reflection.FieldInfo[] fields = typeof(LanguageDefine).GetFields();
//            List<string> listString = new List<string>();
//            foreach (var item in fields)
//            {
//                string fieldName = item.Name;
//                if (fieldName == "ID") continue;
//                listString.Add(fieldName);
//            }
//            knownLanguages = listString.ToArray();
//            listString = null;

//            string pairsKey;
//            Dictionary<string, string[]> pairs = new Dictionary<string, string[]>();
//            foreach (var item in config.Language)
//            {
//                pairsKey = item.ID.ToString();
//                string[] v;
//                if (pairs.TryGetValue(pairsKey, out v)) continue;//Array.Resize<string>(ref v, 0);
//                else
//                {
//                    v = new string[] { item.Chinese, item.English };
//                    pairs[pairsKey] = v;
//                }
//            }
//            dictionary = pairs;
//            pairs = null;
//        }
//    }

//    public static void ExecuteLocalize(UILocalize localize, string context)
//    {
//        if (localize == null) return;
//        switch (localize.localizeType)
//        {
//            case UILocalizeType.Text:
//                localize.SetText(context);
//                break;
//            case UILocalizeType.ImageLoad:
//                UnityEngine.Sprite sprite = null;
//                string[] paths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName("icon.unity3d", context);
//                if (paths == null || paths.Length <= 0)
//                {
//                    UnityEngine.Debug.LogError($"{localize.gameObject.name}的语言表id填写错误,找不到对应的sprite资源");
//                }
//                else
//                {
//                    sprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Sprite>(paths[0]);
//                }
//                localize.SetImage(sprite);
//                break;
//            case UILocalizeType.Font:
//                UnityEngine.Font font = AssetDatabase.LoadAssetAtPath<UnityEngine.Font>(context);
//                localize.SetFont(font);
//                break;
//            case UILocalizeType.Audio:
//                UnityEngine.AudioClip audioClip = AssetDatabase.LoadAssetAtPath<UnityEngine.AudioClip>(context);
//                localize.SetAudio(audioClip);
//                break;
//            default:
//                break;
//        }
//    }

//    public static void UpdatedConfig()
//    {
//        localizationHasBeenSet = false;
//        keysHasBeenSet = false;
//    }
//}
//#endif