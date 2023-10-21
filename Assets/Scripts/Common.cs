using System;
using UnityEngine;

public class Common
{

    public enum  GameState
    {
        Start = 0,
        Playing,
        Result,
    }
    
    public enum TargetType
    {
        None,               // none
        Solider,            // С??
        BigSolider,         // ???
        Build,              // ????
    }


    public enum CampType{
        Red,
        Bule,
    }


    public enum ActionType{
        Find,
        Move,
        Die,
        Attack
    }

    public struct AIAction{
        public CampType camp;
        public ActionType type;
        public RVOAgent targetRVOAgent;
    }


    /// <summary>
    /// ?????????
    /// </summary>
    public enum EventCmd
    {
        None,
        NoticeMsg,              // 服务器出兵通知
        ResetGame,             // 重启游戏
        SubBuildHp,             // 建筑减血
        RankUpdate,            // 排名更新
    }

    public enum Constance
    {
        AttackDis = 0,          // 攻击间隔
        DefaultCreateCount,     // 默认创建数量
    }

    /// <summary>
    /// 怪物数据
    /// </summary>
    public class MasterData
    {

        /// <summary>
        /// ????ID
        /// </summary>
        public int ID;

        /// <summary>
        /// ???
        /// </summary>
        public CampType CampType;

        /// <summary>
        /// ????????
        /// </summary>
        public TargetType TargetType;

        /// <summary>
        /// ????
        /// </summary>
        /// <returns></returns>
        public int Count;


    }
    
    public static void Slog(string mes)
    {
        Debug.Log(mes);
    }

    public static void SError(string msg)
    {
        Debug.LogError(msg);
    }
}


public class PlayerInfoData
{
    /// <summary>
    /// ??
    /// </summary>
    public int Index ;

    /// <summary>
    /// ??
    /// </summary>
    public string Name;

    /// <summary>
    /// ??
    /// </summary>
    public int Score;

    /// <summary>
    /// ??
    /// </summary>
    public string HeadIcon;

    
    public PlayerInfoData(int index,string name,int score,string headIcon)
    {
        Index = index;
        Name = name;
        Score = score;  
        HeadIcon = headIcon;    
    }

  
}