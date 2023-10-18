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