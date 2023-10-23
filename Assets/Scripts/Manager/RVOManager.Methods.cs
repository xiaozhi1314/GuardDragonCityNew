using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RVO;
using DG.Tweening;
using BigDream;

public sealed partial class RVOManager : MonoSingleton<RVOManager>
{
   
    // 移除小兵
    public void RemoveSolider(int sid, Common.CampType CampType)
    {
        if(CampType == Common.CampType.Red)
        {
            if(leftSoliderAgent.ContainsKey(sid))
            {
                leftSoliderAgent.Remove(sid);
                Simulator.Instance.delAgent(sid);
            }
            
        }
        else if(CampType == Common.CampType.Bule)
        {
            if(rightSoliderAgent.ContainsKey(sid))
            {
                rightSoliderAgent.Remove(sid);
                Simulator.Instance.delAgent(sid);
            }
        }
    }

    /// <summary>
    /// 删除所有soldier
    /// </summary>
    public void RemoveAllSolider()
    {
        leftSoliderAgent.Values.ToList().ForEach(agent =>
        {
            PoolManager.Instance.FreeObj(agent.m_GameData.PoolName, agent.gameObject);
        });
        
        rightSoliderAgent.Values.ToList().ForEach(agent =>
        {
            PoolManager.Instance.FreeObj(agent.m_GameData.PoolName, agent.gameObject);
        });
        leftSoliderAgent.Clear();
        rightSoliderAgent.Clear();
    }


    // 根据sid和阵营获取某个小兵
    public RVOAgent GetSolider(int sid, Common.CampType CampType)
    {

         if(CampType == Common.CampType.Red)
        {
            if(leftSoliderAgent.ContainsKey(sid))
            {
               return leftSoliderAgent[sid];
            }
            if(RedBuildAgent.m_GameData.Sid == sid)
            {
                return RedBuildAgent;
            }
            
        }
        else if(CampType == Common.CampType.Bule)
        {
            if(rightSoliderAgent.ContainsKey(sid))
            {
               return rightSoliderAgent[sid];
            }

             if(BlueBuildAgent.m_GameData.Sid == sid)
            {
                return BlueBuildAgent;
            }
        }
        return null;
    }

    // 获取某个阵营小兵的数量
    public int GetSoliderCount( Common.CampType CampType)
    {
         if(CampType == Common.CampType.Red)
        {
            return leftSoliderAgent.Count;
            
        }
        else if(CampType == Common.CampType.Bule)
         {
             return rightSoliderAgent.Count;
         }
        return 0;
    }

    
    // 获取某个阵营对方基地
    public RVOAgent GetBuildSolider( Common.CampType CampType)
    {
         if(CampType == Common.CampType.Red)
        {
            return RedBuildAgent;
            
        }
        else if(CampType == Common.CampType.Bule)
         {
             return BlueBuildAgent;
         }
        return null;
    }
    
    
     // 创建小兵
        public void CreateSolider(int ID, int count, Common.CampType campType, string tikTokId = "")
        {
            var tableGameData = TableManager.Instance.GetArrayData<TableMasterData>(ID);
            SetAgentDefaults(tableGameData.AgentDefaults);
            Common.CampType emptyCampType = (campType == Common.CampType.Red) ? Common.CampType.Bule : Common.CampType.Red;
            for(int i = 0; i < 1; i++)
            {
                for(int j = 0; j < count; j++)
                {
                    float x = 0;
                    if (campType == Common.CampType.Red)
                    {
                        x  =  -80 + i * 3;
                    }
                    else
                    {
                        x=  80 - i * 3;
                    }
    
                    float z = 15 - j * 3;
                    int sid = Simulator.Instance.addAgent(new RVO.Vector2(x, z));
                    if(sid >= 0)
                    {
                        var gameData = GetGameData(ID , Common.TargetType.BigSolider, sid, campType, emptyCampType, BlueBuildAgent,tikTokId);
                        GameObject tmp = PoolManager.Instance.GetObj(gameData.PoolName, transform, new Vector3(x, 1f, z));

                        tmp.name = "solider" + sid;
                        var rVOAgent = tmp.GetComponent<RVOAgent>();
                        rVOAgent.initData(gameData);
                        Simulator.Instance.setAgentPrefVelocity(sid,  (new RVO.Vector2(x,z)));
                        if (campType == Common.CampType.Red)
                        {
                            leftSoliderAgent.Add(sid, rVOAgent);
                        }
                        else
                        {
                            rightSoliderAgent.Add(sid, rVOAgent);
                        }
                    }
                }
            }
        }
    
}
