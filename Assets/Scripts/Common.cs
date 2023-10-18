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