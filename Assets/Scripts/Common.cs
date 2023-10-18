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
        Solider,            // Ð¡±ø
        BigSolider,         // ´ó±ø
        Build,              // ½¨Öþ
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

    /// <summary>
    /// ï¿½ï¿½Ï¢Í¨Öªï¿½ï¿½ï¿½ï¿½
    /// </summary>
    public enum EventCmd
    {
        None,
        AddMaster,
    }

    /// <summary>
    /// ï¿½ï¿½Ê¼ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
    /// </summary>
    public class MasterData
    {

        /// <summary>
        /// ï¿½ï¿½ï¿½ï¿½ID
        /// </summary>
        public int ID;

        /// <summary>
        /// ï¿½ï¿½Óª
        /// </summary>
        public CampType CampType;

        /// <summary>
        /// ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
        /// </summary>
        public TargetType TargetType;

        /// <summary>
        /// ï¿½ï¿½ï¿½ï¿½
        /// </summary>
        /// <returns></returns>
        public int Count;


    }
}