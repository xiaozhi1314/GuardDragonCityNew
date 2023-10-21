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
        Solider,            // 小兵
        BigSolider,         // 大兵
        Build,              // 建筑
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
    /// 消息通知类型
    /// </summary>
    public enum EventCmd
    {
        None,
        AddMaster,
        AddSolider,
        AddBigSolider,
        ResetGame,              // 重开游戏 
        SubBuildHp,             // 减少血量
        RankUpdate,             // 排行更新
    }

    public enum Constance
    {
        AttackDis = 0,          // 出兵间隔
        DefaultCreateCount,     // 默认创建数量
    }

    /// <summary>
    /// 初始化怪物数据
    /// </summary>
    public class MasterData
    {

        /// <summary>
        /// 怪物ID
        /// </summary>
        public int ID;

        /// <summary>
        /// 阵营
        /// </summary>
        public CampType CampType;

        /// <summary>
        /// 怪物类型
        /// </summary>
        public TargetType TargetType;

        /// <summary>
        /// 数量
        /// </summary>
        /// <returns></returns>
        public int Count;


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