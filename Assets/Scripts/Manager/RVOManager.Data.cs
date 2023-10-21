using System.Collections.Generic;
using UnityEngine;
using RVO;
using DG.Tweening;
using BigDream;
using Unity.VisualScripting;
using static Common;

public sealed partial class RVOManager : MonoSingleton<RVOManager>
{
    /// <summary>
    /// 设置兵的大小
    /// </summary>
    /// <param name="targetType"></param>
    public void SetAgentDefaults(Common.TargetType targetType)
    { 
        switch (targetType)
        {
            case Common.TargetType.Solider:
                Simulator.Instance.setAgentDefaults(10.0f, 10, 5.0f, 5.0f, 1.0f, 2.0f, new RVO.Vector2(0.0f, 0.0f));
                break;
            case Common.TargetType.BigSolider:
                Simulator.Instance.setAgentDefaults(10.0f, 10, 5.0f, 5.0f, 1.0f, 2.0f, new RVO.Vector2(0.0f, 0.0f));
                break;
            case Common.TargetType.Build:
                Simulator.Instance.setAgentDefaults(50.0f, 100, 5.0f, 5.0f, 1.0f, 2.0f, new RVO.Vector2(0.0f, 0.0f));
                break;
            default:
                Simulator.Instance.setAgentDefaults(10.0f, 10, 5.0f, 5.0f, 1.0f, 2.0f, new RVO.Vector2(0.0f, 0.0f));
                break;
        }
    }

    /*
    public GameData GetGameData(Common.TargetType targetType, int sid, Common.CampType campType, Common.CampType emptyCampType, RVOAgent argetAgent = null)
    {
        var gameData = new GameData();
        gameData.TargetType = Common.TargetType.Solider;
        gameData.CampType = campType;
        gameData.EmtpyCampType = emptyCampType;
        gameData.Sid = sid;
        gameData.TargetAgent = argetAgent;

        if (targetType == Common.TargetType.Solider)
        {
            gameData.HP = 10;
            gameData.MaxHp = 10;
            gameData.Atk = 1;
            gameData.AtkCd = 0.5f;
            gameData.AtkDis = 5.0f;
            gameData.FindDis = 10.0f;
            gameData.FindCD = 0.1f;
        }
        else if (targetType == Common.TargetType.BigSolider)
        {
            gameData.HP = 20;
            gameData.MaxHp = 100;
            gameData.Atk = 10;
            gameData.AtkCd = 1.0f;
            gameData.AtkDis = 10.0f;
            gameData.FindDis = 20.0f;
            gameData.FindCD = 0.2f;

        }
        else if (targetType == Common.TargetType.Build)
        {
            gameData.HP = 100000;
            gameData.MaxHp = 100000;
            gameData.Atk = 1;
            gameData.AtkCd = 0.5f;
            gameData.AtkDis = 10.0f;
            gameData.FindDis = 10.0f;
            gameData.FindCD = 0.1f;

        }
        return gameData;
    }
    */

    public GameData GetGameData(int index, Common.TargetType targetType, int sid, Common.CampType campType, Common.CampType emptyCampType, RVOAgent argetAgent = null, string tikTokId = "")
    {
        var gameData = new GameData();
      
        gameData.CampType = campType;
        gameData.EmtpyCampType = emptyCampType;
        gameData.Sid = sid;
        gameData.TargetAgent = argetAgent;
        gameData.TikTokId = tikTokId;
        
        var tableMasterData  = TableManager.Instance.GetArrayData<TableMasterData>(index);
        gameData.TargetType = tableMasterData.AgentDefaults;
        gameData.HP = tableMasterData.HP;
        gameData.MaxHp = tableMasterData.MaxHP;
        gameData.Atk = tableMasterData.Atk;
        gameData.AtkCd = tableMasterData.AtkCD;
        gameData.AtkDis = tableMasterData.AtkDis;
        gameData.FindDis = tableMasterData.FindDis;
        gameData.FindCD = tableMasterData.FindCD;
        gameData.PoolName = tableMasterData.PoolName;
        gameData.Score = tableMasterData.Score;
        return gameData;
    }

}
