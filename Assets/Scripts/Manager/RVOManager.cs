using System.Collections.Generic;
using UnityEngine;
using RVO;
using DG.Tweening;
using BigDream;
using System.Diagnostics;
using Newtonsoft.Json;

public sealed partial class RVOManager : MonoSingleton<RVOManager>
{


    public override void Init()
    {
        Simulator.Instance.setTimeStep(0.25f);

        SetAgentDefaults(Common.TargetType.Build);
        InitBuild(1, RedBuildAgent, Common.CampType.Red, Common.CampType.Bule);
        InitBuild(2, BlueBuildAgent, Common.CampType.Bule, Common.CampType.Red);
        EventManager.Instance.Subscribe(Common.EventCmd.AddSolider, this, EventAddSolider);
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


    private void EventAddSolider(object sender = null, object userData = null, EventParams e = null){
        if(e.Objects.Count > 0){
            var masterData = JsonConvert.DeserializeObject<Common.MasterData>((string)e.Objects["Data"]);
            CreateSolider( masterData.ID, masterData.Count, masterData.CampType);
        }
    }

    
    /// <summary>
    /// 自动创建小兵
    /// </summary>
    public void AutoCreateSolider()
    {
        var index = 0;
        DOTween.Sequence().AppendInterval(2.0f).AppendCallback(() =>{
            SetAgentDefaults(Common.TargetType.Solider);
            if (index % 2 == 0)
            {
                CreateSolider( 4, 10, Common.CampType.Bule);
                CreateSolider( 3, 10, Common.CampType.Red);
            }
            else
            {
                CreateSolider( 3, 10, Common.CampType.Red);
                CreateSolider( 4, 10, Common.CampType.Bule);
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
