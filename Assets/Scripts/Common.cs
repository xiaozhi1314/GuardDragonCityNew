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
        Solider,            // С��
        BigSolider,         // ���
        Build,              // ����
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
    /// ��Ϣ֪ͨ����
    /// </summary>
    public enum EventCmd
    {
        None,
        AddMaster,
    }

    /// <summary>
    /// ��ʼ����������
    /// </summary>
    public class MasterData
    {

        /// <summary>
        /// ����ID
        /// </summary>
        public int ID;

        /// <summary>
        /// ��Ӫ
        /// </summary>
        public CampType CampType;

        /// <summary>
        /// ��������
        /// </summary>
        public TargetType TargetType;

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int Count;


    }

}