using System.Collections.Generic;
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


}
