/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EventParams.cs
 * author:    taoye
 * created:   2021/1/8
 * descrip:   事件参数
 ***************************************************************/

using System.Collections.Generic;
using BigDream;

public class EventParams
{
    /// <summary>
    ///     事件类型编号
    /// </summary>
    public Common.EventCmd Cmd { set; get; }

    /// <summary>
    ///     事件参数
    /// </summary>
    public Dictionary<string, object> Objects { set; get; }

    public EventParams(Common.EventCmd cmd, Dictionary<string, object> objects = null)
    {
        Cmd = cmd;
        Objects = objects;
    }

    /// <summary>
    ///     清理引用
    /// </summary>
    public void Clear()
    {
        Cmd = Common.EventCmd.None;
        Objects = null;
    }

    /// <summary>
    ///     获取Bool值
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    public bool GetBool(string name)
    {
        if (Objects != null)
            if (Objects.ContainsKey(name))
                return bool.Parse(Objects[name].ToString());
        return false;
    }

    /// <summary>
    ///     获取Int值
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    public int GetInt(string name)
    {
        if (Objects != null)
            if (Objects.ContainsKey(name))
                return int.Parse(Objects[name].ToString());
        return 0;
    }

    /// <summary>
    ///     获取Float值
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    public float GetFloat(string name)
    {
        if (Objects != null)
            if (Objects.ContainsKey(name))
                return float.Parse(Objects[name].ToString());
        return 0;
    }

    /// <summary>
    ///     获取String值
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    public string GetString(string name)
    {
        if (Objects != null)
            if (Objects.ContainsKey(name))
                return Objects[name].ToString();
        return string.Empty;
    }

    /// <summary>
    ///     获取object
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    public object GetObject(string name)
    {
        if (Objects != null)
            if (Objects.ContainsKey(name))
                return Objects[name];
        return null;
    }
}