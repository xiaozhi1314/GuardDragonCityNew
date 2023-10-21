using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BigDream;
using DG.Tweening.Plugins.Options;
using RVO;
using Unity.VisualScripting;
using UnityEngine;

public class RVOAgent : MonoBehaviour
{
    
    /// <summary>
    /// 数据
    /// </summary>
    public GameData m_GameData;

    /// <summary>
    /// 设置是否已经死亡
    /// </summary>
    public bool m_IsDie = false;

    /// <summary>
    /// 当前攻击间隔
    /// </summary>
    public float curAttackTime = 0;

    /// <summary>
    /// 当前查找间隔
    /// </summary>
    public float curFindTime = 0;
    
    // 击杀数量
    public int killCount = 0;


    public bool m_IsInit = false;


    public virtual void Proc() { }
    public virtual void SetDie() { }

    public void initData(GameData gameData)
    {
        m_GameData = gameData;
        m_IsInit = true;
        m_IsDie = false;
        curAttackTime = 0;
        curFindTime = 0;
        killCount = 0;
    }

    /// <summary>
    /// 检查是否已经死亡
    /// </summary>
    /// <returns></returns>
    public bool CheckIsDie()
    {
        return m_GameData.ActionType == Common.ActionType.Die;
    }

    // 设置小兵死亡
    public void SetAgentDie()
    {
        if (m_IsDie == false)
        {
            m_IsDie = true;
            SetDie();
            m_IsInit = false;
        }
    }


    void LateUpdate()
    {
        if (GameManager.Instance.GameState != Common.GameState.Playing) return;
        if (m_IsDie || m_IsInit == false) return;
        Proc();
        
    }


    // 减少血量
    public virtual void SubHp(RVOAgent rVOAgent, float attack)
    {
        if (rVOAgent != null && rVOAgent.CheckIsDie() == false && CheckIsDie() == false) 
        {
            m_GameData.HP -= attack;
            // 当打我的人在我的攻击范围内，且我自己还处于移动状态，则直接进行对战状态
            if ((m_GameData.ActionType == Common.ActionType.Move || m_GameData.ActionType == Common.ActionType.Find) && Vector3.Distance(rVOAgent.transform.position, transform.position) <= m_GameData.AtkDis)
            {
                m_GameData.TargetAgent = rVOAgent;
                ChangeToAttackAction();
            }
        }
    }

    // 获取当前血量
    public float GetHp()
    {
        return m_GameData.HP;
    }
   
   public void ChangeToMoveAction()
   {
        m_GameData.ActionType = Common.ActionType.Move;
   }

    // 士兵移动
    public void MoveAciton()
    {
        if(m_GameData.TargetAgent != null && m_GameData.TargetAgent.CheckIsDie() == false)
        {
            RVO.Vector2 pos = Simulator.Instance.getAgentPosition(m_GameData.Sid);  
            RVO.Vector2 vel = Simulator.Instance.getAgentVelocity(m_GameData.Sid);
            try
            {
                transform.position = new Vector3(pos.x(), transform.position.y, pos.y());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            
            if(Mathf.Abs(vel.x()) > 0.01f && Mathf.Abs(vel.y()) > 0.01f)
            {
                transform.forward = new Vector3(vel.x(), 0, vel.y()).normalized;
            }

            RVO.Vector2 goalVector = (new RVO.Vector2(m_GameData.TargetAgent.transform.position.x, m_GameData.TargetAgent.transform.position.z) - pos); // (new RVO.Vector2(0, 0) - Simulator.Instance.getAgentPosition(sid));
            if(RVOMath.absSq(goalVector) > 1.0f)
            {
                goalVector = RVOMath.normalize(goalVector) * 1.5f;
            }
            Simulator.Instance.setAgentPrefVelocity(m_GameData.Sid, goalVector);

            // 到达目标更新状态
            if(Vector3.Distance(m_GameData.TargetAgent.transform.position, transform.position) <= m_GameData.AtkDis)
            {
                ChangeToAttackAction();
                return;
            }
        }
        else // 寻路过程中人死亡，则从新定位到敌方基地
        {
            m_GameData.ActionType = Common.ActionType.Find;
        }
        
    }

    // 从其他状态变成Attack
    public void ChangeToAttackAction()
    {

        m_GameData.ActionType = Common.ActionType.Attack;
        Simulator.Instance.setAgentPrefVelocity(m_GameData.Sid, new RVO.Vector2(0.0f, 0.0f));
        curAttackTime = 0;
    }

    // 攻击action
    public virtual void AttackAction()
    {
        curAttackTime += Time.deltaTime;
        if (curAttackTime < m_GameData.AtkCd) return;
        curAttackTime = 0;
        if(m_GameData.TargetAgent != null && m_GameData.TargetAgent.CheckIsDie() == false) // 非死亡状态
        {
            Debug.Log("播放攻击动画 ");
            m_GameData.TargetAgent.SubHp(this, m_GameData.Atk);

            if(m_GameData.TargetAgent.GetHp() <= 0) // 血量是0时设置对方已经死亡
            {
                killCount++;
                m_GameData.TargetAgent.m_GameData.ActionType = Common.ActionType.Die;
            }
        }
        else
        {
            m_GameData.ActionType = Common.ActionType.Find;
        }

    }


    // 移动过程中发现相邻目标进行攻击
    public void FindAttackTarget()
    {
        curFindTime += Time.deltaTime;
        if (curFindTime < m_GameData.FindCD) return;
        curFindTime = 0;

        // 查找最近的邻居，发现有对方的小兵直接进行攻击
        int count = Simulator.Instance.getAgentNumAgentNeighbors(m_GameData.Sid);
        if( count > 0)
        {
            RVOAgent target = null;
            float minDistance  = 1000;
            for(int idx = 0; idx < count; idx ++)
            {
                int findSid = Simulator.Instance.getAgentAgentNeighbor(m_GameData.Sid, idx);
                var findAgent = RVOManager.Instance.GetSolider(findSid,m_GameData.EmtpyCampType);
                if(findAgent != null)
                {
                    var distance = Vector3.Distance(findAgent.transform.position, transform.position);
                    if(distance <= m_GameData.FindDis && distance < minDistance ) // 在警戒范围内,且查找出最小距离的小兵
                    {
                        target = findAgent;
                        minDistance = distance;
                    }
                
                }
            }

            if(target != null)
            {
                m_GameData.TargetAgent = target;
            }
            if(RVOManager.Instance.GetSoliderCount(m_GameData.EmtpyCampType) == 0) // 敌方小兵全部死亡
            {
                m_GameData.TargetAgent = RVOManager.Instance.GetBuildSolider(m_GameData.EmtpyCampType); // 直接寻路敌方基地
            }

        }
    }


    // 建筑的查找
    public void FindBuildAttackTarget()
    {
        curFindTime += Time.deltaTime;
        if (curFindTime < m_GameData.FindCD) return;
        curFindTime = 0;
        // 查找最近的邻居，发现有对方的小兵直接进行攻击
        int count = Simulator.Instance.getAgentNumAgentNeighbors(m_GameData.Sid);
        if( count > 0)
        {
            RVOAgent target = null;
            float minDistance  = 1000;
            for(int idx = 0; idx < count; idx ++)
            {
                int findSid = Simulator.Instance.getAgentAgentNeighbor(m_GameData.Sid, idx);
                var findAgent = RVOManager.Instance.GetSolider(findSid,m_GameData.EmtpyCampType);
                if(findAgent != null)
                {
                    var distance = Vector3.Distance(findAgent.transform.position, transform.position);
                    if(distance <= m_GameData.AtkDis && distance < minDistance ) // 在攻击范围内,且查找出最小距离的小兵
                    {
                        target = findAgent;
                        minDistance = distance;
                    }
                
                }
            }

            if(target != null)
            {
                m_GameData.TargetAgent = target;
                ChangeToAttackAction();
            }
        
        }
    }

}
