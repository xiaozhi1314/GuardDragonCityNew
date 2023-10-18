using System.Collections.Generic;
using UnityEngine;
using RVO;
using DG.Tweening;
using BigDream;

public sealed partial class RVOManager : MonoSingleton<RVOManager>
{


    public override void Init()
    {
        string gameConfigJson = Resources.Load<TextAsset>("Config/GameConfig").text;
        gameConfig = JsonUtility.FromJson<GameConfig>(gameConfigJson);

        Simulator.Instance.setTimeStep(0.25f);

        SetAgentDefaults(Common.TargetType.Build);
        InitBuild(RedBuildAgent, Common.CampType.Red, Common.CampType.Bule);
        InitBuild(BlueBuildAgent, Common.CampType.Bule, Common.CampType.Red);

        var index = 0;
        DOTween.Sequence().AppendInterval(2.0f).AppendCallback(() =>{
            SetAgentDefaults(Common.TargetType.Solider);
            if (index % 2 == 0)
            {
                CreateSolider(redPrefab, Common.CampType.Red, Common.CampType.Bule);
                // CreateSolider(bluePrefab, Common.CampType.Bule, Common.CampType.Red);
            }
            else
            {
                // CreateSolider(bluePrefab, Common.CampType.Bule, Common.CampType.Red);
                CreateSolider(redPrefab, Common.CampType.Red, Common.CampType.Bule);
            }

      
        }).SetLoops(200);


        DOTween.Sequence().AppendInterval(5.0f).AppendCallback(() =>
        {
            int camp = Random.Range(1,3);
            int bing = Random.Range(1,7);
            if(camp == 1){
                GameObject dabing1fab = Resources.Load<GameObject>("Prefabs/Gift/Left/Dabing" + bing.ToString());
                CreateBigSolider(dabing1fab, Common.CampType.Red, Common.CampType.Bule);
            }else if(camp == 2){
                GameObject dabing1fab = Resources.Load<GameObject>("Prefabs/Gift/Right/Dabing" + bing.ToString());
                CreateBigSolider(dabing1fab, Common.CampType.Bule, Common.CampType.Red);
            }

        }).SetLoops(2);

        DOTween.Sequence().AppendInterval(3.0f).AppendCallback(() =>
        {
            GameState = Common.GameState.Playing;
        });
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

    
    public void CreateBigSolider(int type, int index)
    {
        Common.CampType campType = (type == 1) ? Common.CampType.Red : Common.CampType.Bule;
        Common.CampType emptyCampType = (type == 1) ? Common.CampType.Bule : Common.CampType.Red;

        float x = campType == Common.CampType.Red ? -90 : 90;
        float z = 15;
        int sid = Simulator.Instance.addAgent(new RVO.Vector2(x, z));
        if(sid >= 0)
        {
            string bigSoliderPrefabPath = "Prefabs/Gift/" + (campType == Common.CampType.Red ? "Left" : "Right") + "/Dabing" + index;
            GameObject prefab = Resources.Load<GameObject>(bigSoliderPrefabPath);
            var gameData = GetGameData(index -1 , Common.TargetType.BigSolider, sid, campType, emptyCampType, BlueBuildAgent);
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
    public void InitBuild(RVOAgent rVOAgent,  Common.CampType campType, Common.CampType emptyCampType)
    {
        var sid = Simulator.Instance.addAgent(new RVO.Vector2(rVOAgent.transform.position.x, rVOAgent.transform.position.z));
        var gameData = GetGameData(0, Common.TargetType.Build, sid, campType, emptyCampType);
        rVOAgent.initData(gameData);

    }

}
