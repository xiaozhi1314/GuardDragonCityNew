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
        Solider,            // §³??
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
        AddMaster,
        AddSolider,
        AddBigSolider,
        ResetGame,              // ?????? 
        SubBuildHp,             // ???????
        RankUpdate,             // ???§Ú???
    }

    public enum Constance
    {
        AttackDis = 0,          // ???????
        DefaultCreateCount,     // ??????????
    }

    /// <summary>
    /// ?????????????
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