using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BigDream;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class TableManager : MonoSingleton<TableManager>
{
    public List<string> _arrTableNames;
    public  List<string> _mapTableNames;
    
    // 队列型数据对象分类集合
    protected Dictionary<Type, Dictionary<int, object>> _arrDatas = new Dictionary<Type, Dictionary<int, object>>();
    
    // 键值型数据对象分类集合
    protected Dictionary<Type, object> _mapDatas = new Dictionary<Type, object>();
    
    public override void Init()
    {
        GFuncs.SLog("TableManager Init");
        
        _arrTableNames = new List <string>
        {
            "TableMaster"
        };
        _mapTableNames = new List<string>();
        
        InitJsonTableData();

       var temp = GetArrayData<TableMasterData>(1);
       
    }
    
    private void InitJsonTableData()
    {
        //GFuncs.SLog("[CONFIG]" + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + ":" + "InitJsonTableData");

        // 队列型数据对象统一加载
        foreach (string jsonName in _arrTableNames)
        {
            Type type = Type.GetType(jsonName + "Data");
            string jsonText = LoadJsonFile($"/Json/{jsonName}.json", true);
            JArray arrContent = JArray.Parse(jsonText);
            _arrDatas[type] = new Dictionary<int, object>();

            for (int index = 0; index < arrContent.Count; index++)
            {
                var content = (JObject)arrContent[index];
                var data = JsonConvert.DeserializeObject(content.ToString(), type);
                _arrDatas[type][(int)content["ID"]] = data;
            }

            GFuncs.SLog("[TABLE]" + jsonName + ".json Init Success!");
            
        }
        
        // 键值型数据对象统一加载
        foreach (string jsonName in _mapTableNames)
        {
            Type type = Type.GetType(jsonName + "Data");
            string jsonText = LoadJsonFile($"/Json/{jsonName}.json", true);

            var data = JsonConvert.DeserializeObject(jsonText, type);
            _mapDatas[type] = data;

            GFuncs.SLog("[TABLE]" + jsonName + ".json Init Success!");
        }

    }
    
    // 获取队列型数据
    public T GetArrayData<T>(int ID) where T : CTableData
    {
        if (null != _arrDatas[typeof(T)] && _arrDatas[typeof(T)].ContainsKey(ID))
        {
            return _arrDatas[typeof(T)][ID] as T;
        }
        return null;
    }

    // 获取队列型数据集合
    public Dictionary<int, object> GetArrayDatasGroup<T>() where T : CTableData
    {
        return _arrDatas[typeof(T)];
    }

    // 获取队列型数据表的长度
    public int GetArrayDatasCount<T>() where T : CTableData
    {
        return _arrDatas[typeof(T)].Count;
    }
    
    
    // 获取键值型数据
    public T GetMapData<T>() where T : CTableData
    {
        if (null != _mapDatas[typeof(T)])
        {
            return _mapDatas[typeof(T)] as T;
        }
        return null;
    }
    

    private string LoadJsonFile(string fileNameWithPath, bool fromStreamingAssets = false)
    {
        if (fromStreamingAssets)
        {
            string path = Application.streamingAssetsPath + "/" + fileNameWithPath;
            string url =
#if UNITY_ANDROID && !UNITY_EDITOR
                        path;
#elif UNITY_IOS && !UNITY_EDITOR
                        "file://" + path;
#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
                "file://" + path;
#else
                        string.Empty;
#endif
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.SendWebRequest();
            while (!www.downloadHandler.isDone) { }
            string result = www.downloadHandler.text;
            www.Dispose();
            return result;
        }
        else
        {
            string path = Application.persistentDataPath + "/" + fileNameWithPath;
            StreamReader reader = null;
            try
            {
                reader = File.OpenText(path);
            }
            catch
            {
                Debug.Log("File don't find! path = " + path);
                return "";
            }
            // 先解析域名
            string content = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            return content;
        }
    }

}

// Table数据单元基类（以后拓展）
public class CTableData
{
    public void SetPropertyValue<T>(string keyName, T value)
    {
        GetType().GetProperty(keyName).SetValue(this, value);
    }

    public T GetPropertyValue<T>(string keyName)
    {
        return (T)GetType().GetProperty(keyName).GetValue(this);
    }

}
