using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BigDream;
using DG.Tweening.Plugins.Options;
using RVO;
using Unity.VisualScripting;
using UnityEngine;

public class RVOAgentBigSolider : RVOAgent
{

    // 设置小兵死亡
    public override void SetDie()
    {
        Debug.Log("播放死亡动画，击杀数：" + killCount + "SID：" + m_GameData.Sid);
        RVOManager.Instance.RemoveSolider(m_GameData.Sid, m_GameData.CampType);
        PoolManager.Instance.FreeObj(m_GameData.PoolName, gameObject);
    }


    public override void  Proc()
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
    

    // 攻击action
    public override void AttackAction()
    {
        curAttackTime += Time.deltaTime;
        if (curAttackTime < m_GameData.AtkCd) return;
        curAttackTime = 0;
        if(m_GameData.TargetAgent != null && m_GameData.TargetAgent.CheckIsDie() == false) // 非死亡状态
        {
            Debug.Log("播放攻击动画-AttackAction");
            // m_GameData.TargetAgent.SubHp(this, m_GameData.Atk);
            AttackTarget();

            // if(m_GameData.TargetAgent.GetHp() <= 0) // 血量是0时设置对方已经死亡
            // {
            //     killCount++;
            //     m_GameData.TargetAgent.m_GameData.ActionType = Common.ActionType.Die;
            // }
        }
        else
        {
            m_GameData.ActionType = Common.ActionType.Find;
        }

    }

    private void AttackTarget(){
        // 查找最近的邻居，发现有对方的小兵直接进行攻击
        int count = Simulator.Instance.getAgentNumAgentNeighbors(m_GameData.Sid);
        if( count > 0)
        {
            for(int idx = 0; idx < count; idx ++)
            {
                int findSid = Simulator.Instance.getAgentAgentNeighbor(m_GameData.Sid, idx);
                var findAgent = RVOManager.Instance.GetSolider(findSid,m_GameData.EmtpyCampType);
                if(findAgent != null)
                {
                    var distance = Vector3.Distance(findAgent.transform.position, transform.position);
                    if(distance <= m_GameData.AtkDis)
                    {
                        findAgent.SubHp(this, m_GameData.Atk);
                        if(findAgent.GetHp() <= 0) // 血量是0时设置对方已经死亡
                        {
                            killCount++;
                            findAgent.m_GameData.ActionType = Common.ActionType.Die;
                            // 我击杀对方获得分数  
                            GameManager.Instance.AddScore(findAgent.m_GameData.TikTokId, findAgent.m_GameData.Score);
                        }
                    }
                
                }
            }
        }
    }
}
