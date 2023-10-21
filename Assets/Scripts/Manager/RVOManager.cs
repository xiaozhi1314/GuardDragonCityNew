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


        DOTween.Sequence().AppendInterval(5.0f).AppendCallback(() =>
        {
            Common.CampType camp = (Random.Range(1,3) == 1) ? Common.CampType.Red : Common.CampType.Bule;
            int bing = Random.Range(5,16);
            CreateSolider( bing, 1, camp);

        }).SetLoops(2);

        DOTween.Sequence().AppendInterval(3.0f).AppendCallback(() =>
        {
            GameState = Common.GameState.Playing;
        });

        EventManager.Instance.Subscribe(Common.EventCmd.AddSolider, this, EventAddSolider);
    }

    // Update is called once per frame
    void Update()
    {
        Simulator.Instance.doStep();
    }
    

    // 初始化建筑
    public void InitBuild(int index, RVOAgent rVOAgent,  Common.CampType campType, Common.CampType emptyCampType)
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
}
