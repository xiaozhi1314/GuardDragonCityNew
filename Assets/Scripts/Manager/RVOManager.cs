using System;
using System.Collections.Generic;
using RVO;
using DG.Tweening;
using BigDream;
using System.Diagnostics;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

public sealed partial class RVOManager : MonoSingleton<RVOManager>
{


    public override void Init()
    {
        Simulator.Instance.setTimeStep(0.25f);

        SetAgentDefaults(Common.TargetType.Build);
        InitBuild(1, RedBuildAgent, Common.CampType.Red, Common.CampType.Bule);
        InitBuild(2, BlueBuildAgent, Common.CampType.Bule, Common.CampType.Red);
    }

    // Update is called once per frame
    void Update()
    {
        Simulator.Instance.doStep();
    }
    

    // 初始化建筑
    private void InitBuild(int index, RVOAgent rVOAgent,  Common.CampType campType, Common.CampType emptyCampType)
    {
        var sid = Simulator.Instance.addAgent(new RVO.Vector2(rVOAgent.transform.position.x, rVOAgent.transform.position.z));
        var gameData = GetGameData(index, Common.TargetType.Build, sid, campType, emptyCampType);
        rVOAgent.initData(gameData);

    }


    /// <summary>
    /// 自动创建小兵
    /// </summary>
    public void AutoCreateSolider()
    {
        var index = 0;
        var atttackDis = TableManager.Instance.GetArrayData<TableConstanceData>((int)Common.Constance.AttackDis).Value;
        var DefaultCreateCount = Convert.ToInt32(TableManager.Instance.GetArrayData<TableConstanceData>((int)Common.Constance.DefaultCreateCount).Value);
        DOTween.Sequence().AppendInterval(atttackDis).AppendCallback(() =>{
            if (index % 2 == 0)
            {
                CreateSolider( 4, DefaultCreateCount, Common.CampType.Bule);
                CreateSolider( 3, DefaultCreateCount, Common.CampType.Red);
            }
            else
            {
                CreateSolider( 3, DefaultCreateCount, Common.CampType.Red);
                CreateSolider( 4, DefaultCreateCount, Common.CampType.Bule);
            }

      
        }).SetLoops(200);
    }

    public void AutoCreateBigSolider()
    {
        DOTween.Sequence().AppendInterval(5.0f).AppendCallback(() =>
        {
            Common.CampType camp = (Random.Range(1,3) == 1) ? Common.CampType.Red : Common.CampType.Bule;
            int bing = (camp == Common.CampType.Red) ? Random.Range(5,10) : Random.Range(11,16);
            CreateSolider( bing, 1, camp);

        }).SetLoops(2);


    }
}
