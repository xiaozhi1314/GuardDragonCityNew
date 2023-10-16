using System;
using UnityEngine;

public class Common
{

    public enum CampType{
        Red,
        Bule,
    }
    public enum ActionType{
        Idle,
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