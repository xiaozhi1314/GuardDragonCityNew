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
                CreateSolider(redPrefab, Common.CampType.Red, Common.CampType.Bule);
                CreateSolider(bluePrefab, Common.CampType.Bule, Common.CampType.Red);
            }
            else
            {
                CreateSolider(bluePrefab, Common.CampType.Bule, Common.CampType.Red);
                CreateSolider(redPrefab, Common.CampType.Red, Common.CampType.Bule);
            }

      
        }).SetLoops(200);


        DOTween.Sequence().AppendInterval(5.0f).AppendCallback(() =>
        {
            Common.CampType camp = (Random.Range(1,3) == 1) ? Common.CampType.Red : Common.CampType.Bule;
            int bing = Random.Range(5,16);
            CreateBigSolider(camp, bing);

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


    // 创建小兵
    public void CreateSolider(GameObject prefab, Common.CampType campType, Common.CampType emptyCampType)
    {
        for(int i = 0; i < 1; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                float x = 0;
                
                if (campType == Common.CampType.Red)
                {
                    x  =  -90 + i * 3;
                }
                else
                {
                    x=  90 - i * 3;
                }

                float z = 15 - j * 3;
                int sid = Simulator.Instance.addAgent(new RVO.Vector2(x, z));
                if(sid >= 0)
                {
                    var gameData = GetGameData(Common.TargetType.Solider, sid, campType, emptyCampType, BlueBuildAgent);
                    GameObject tmp = GameObject.Instantiate(prefab, new Vector3(x, 1f, z), Quaternion.identity);
                    tmp.name = "agent" + sid;
                    var rVOAgent = tmp.GetComponent<RVOAgentSolider>();
                    rVOAgent.initData(gameData);
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

    
    // 创建小兵
    public void CreateSolider(int index, Common.CampType campType)
    {
        float x = (campType == Common.CampType.Red ? -90 : 90);
        float z = 15;
        var tableMasterData  = TableManager.Instance.GetArrayData<TableMasterData>(index);
        var prefab = Resources.Load<GameObject>(tableMasterData.PrefabPath);
        int sid = Simulator.Instance.addAgent(new RVO.Vector2(x, z));
        if(sid >= 0)
        {
            var emptyCampType = (campType == Common.CampType.Red ? Common.CampType.Bule : Common.CampType.Red);
            var gameData = GetGameData(Common.TargetType.Solider, sid, campType, emptyCampType);
            GameObject tmp = GameObject.Instantiate(prefab, new Vector3(x, 1f, z), Quaternion.identity);
            tmp.name = "agent" + sid;
            var rVOAgent = tmp.GetComponent<RVOAgentSolider>();
            rVOAgent.initData(gameData);
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

    public void CreateBigSolider(GameObject prefab, Common.CampType campType, Common.CampType emptyCampType)
    {
        float x = 0;
        if (campType == Common.CampType.Red)
        {
            x  =  -90;
        }
        else
        {
            x=  90;
        }

        float z = 15;
        int sid = Simulator.Instance.addAgent(new RVO.Vector2(x, z));
        if(sid >= 0)
        {
            var gameData = GetGameData(Common.TargetType.BigSolider, sid, campType, emptyCampType, BlueBuildAgent);
            GameObject tmp = GameObject.Instantiate(prefab, new Vector3(x, 1f, z), Quaternion.identity);
            tmp.name = "dabing" + sid;
            var rVOAgent = tmp.GetComponent<RVOAgentBigSolider>();
            rVOAgent.initData(gameData);
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

    
    public void CreateBigSolider(Common.CampType campType, int index)
    {
        Common.CampType emptyCampType = (campType == Common.CampType.Red) ? Common.CampType.Bule : Common.CampType.Red;
        float x = campType == Common.CampType.Red ? -90 : 90;
        float z = 15;
        int sid = Simulator.Instance.addAgent(new RVO.Vector2(x, z));
        if(sid >= 0)
        {
            var tableMasterData  = TableManager.Instance.GetArrayData<TableMasterData>(index);
            var prefab = Resources.Load<GameObject>(tableMasterData.PrefabPath);
            var gameData = GetGameData(index , Common.TargetType.BigSolider, sid, campType, emptyCampType, BlueBuildAgent);
            GameObject tmp = GameObject.Instantiate(prefab, new Vector3(x, 1f, z), Quaternion.identity);
            tmp.name = "dabing" + sid;
            var rVOAgent = tmp.GetComponent<RVOAgentBigSolider>();
            rVOAgent.initData(gameData);
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
            CreateBigSolider(masterData.CampType, masterData.ID);
        }
    }
}
