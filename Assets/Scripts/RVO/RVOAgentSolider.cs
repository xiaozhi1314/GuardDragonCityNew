using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BigDream;
using DG.Tweening.Plugins.Options;
using RVO;
using Unity.VisualScripting;
using UnityEngine;

public class RVOAgentSolider : RVOAgent
{


    // 设置小兵死亡
    public override void SetDie()
    {
        RVOManager.Instance.RemoveSolider(m_GameData.Sid, m_GameData.CampType);
        PoolManager.Instance.FreeObj(m_GameData.PoolName, gameObject);
        // Debug.Log("播放死亡动画");
    }


    public override void Proc()
    {
        if (m_GameData.ActionType == Common.ActionType.Find) // AI寻路
        {
           SetTargetAgent(RVOManager.Instance.GetBuildSolider(m_GameData.EmtpyCampType));
            ChangeToMoveAction();
        }
        else if (m_GameData.ActionType == Common.ActionType.Move) // 移动
        {
            FindAttackTarget();
            MoveAciton();
        }
        else if (m_GameData.ActionType == Common.ActionType.Attack) // 停下攻击
        {
            AttackAction();
        }
        else if (m_GameData.ActionType == Common.ActionType.Die)  // 死亡
        {
            SetAgentDie();
        }

    }

}
