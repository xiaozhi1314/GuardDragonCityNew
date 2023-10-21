using System;
using UnityEngine;

[Serializable]
public class GameData
{
     /// <summary>
     /// 建筑类型
     /// </summary>
     public Common.TargetType TargetType = Common.TargetType.None;

     /// <summary>
     /// 我方阵营
     /// </summary>
     public Common.CampType CampType = Common.CampType.Bule;


     /// <summary>
     /// 敌方阵营
     /// </summary>
     public Common.CampType EmtpyCampType = Common.CampType.Red;

     /// <summary>
     /// RVO sid
     /// </summary>
     public int Sid = -1;
     
     /// <summary>
     /// 血量
     /// </summary>
     public float HP = 30;
     
     /// <summary>
     /// 最大血量
     /// </summary>
     public float MaxHp = 30;
     
     /// <summary>
     /// 攻击
     /// </summary>
     public float Atk = 1;
     
     /// <summary>
     /// 攻击cd
     /// </summary>
     public float AtkCd = 0.5f;
     
     /// <summary>
     /// 攻击距离
     /// </summary>
     public float AtkDis = 5.0f;

     // 警戒距离
     public float FindDis = 10.0f;

     /// <summary>
     /// 警戒cd
     /// </summary>
     public float FindCD = 0.5f;
     
     /// <summary>
     /// 对象吃名字
     /// </summary>
     public  string PoolName = String.Empty;

    
     /// <summary>
     /// agent状态
     /// </summary>
     public Common.ActionType ActionType = Common.ActionType.Find;

     /// <summary>
     /// 目标agent
     /// </summary>
     public RVOAgent TargetAgent;
     
}