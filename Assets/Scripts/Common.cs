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
        Solider,            // ????
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
    
    public enum AnimationName
    {
        Idle ,
        Run,
        Attack,
        Vectory,
        Die,
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
        NoticeMsg,              // ????????????
        ResetGame,             // ???????
        SubBuildHp,             // ???????
        RankUpdate,            // ????????
        RankResult,
    }

    public enum Constance
    {
        AttackDis = 0,          // ???????
        DefaultCreateCount,     // ??????????
        RVOTimeStep,            // RVO???????
    }

    /// <summary>
    /// ????????
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

    public class DOTweenString
    {
        public static string NoticeUIDelayString = "NoticeUIDelayString";
    }
}


public class PlayerInfoData
{
    /// <summary>
    /// ???????????
    /// </summary>
    public int Index ;
    

    /// <summary>
    /// 
    /// </summary>
    public int Score;

    /// <summary>
    /// tiktokID
    /// </summary>
    public string TikTokId;

    
    public PlayerInfoData(int index,string tikTokId,int score)
    {
        Index = index;
        TikTokId = tikTokId;
        Score = score;     
    }

  
}