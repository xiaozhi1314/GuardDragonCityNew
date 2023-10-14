using System.Collections.Generic;
using UnityEngine;
using RVO;
using DG.Tweening;

public class RVOManager : MonoBehaviour
{
    public GameObject redPrefab, bluePrefab;
    public int count = 0;
    public Dictionary<int, RVOAgent> leftSoliderAgent = new Dictionary<int, RVOAgent>();
    public Dictionary<int, RVOAgent> rightSoliderAgent = new Dictionary<int, RVOAgent>();

    // Start is called before the first frame update
    void Start()
    {
        Simulator.Instance.setTimeStep(0.25f);
        Simulator.Instance.setAgentDefaults(10.0f, 10, 5.0f, 5.0f, 2.0f, 2.0f, new RVO.Vector2(0.0f, 0.0f));
        Simulator.Instance.processObstacles();

        DOTween.Sequence().AppendInterval(2.0f).AppendCallback(() =>{
            CreateSolider();
        }).SetLoops(10);
    }

    // Update is called once per frame
    void Update()
    {
        Simulator.Instance.doStep();
    }


    void CreateSolider(){
        // left solier
        for(int i = 0; i < 10; i++){
            for(int j = 0; j < 10; j++){
                float x = -90 + i * 3;
                float z = 15 - j * 3;
                int sid = Simulator.Instance.addAgent(new RVO.Vector2(x, z));
                if(sid >= 0){
                    GameObject tmp = GameObject.Instantiate(redPrefab, new Vector3(x, 1f, z), Quaternion.identity);
                    tmp.name = "agent" + sid;
                    RVOAgent rVOAgent = tmp.GetComponent<RVOAgent>();
                    rVOAgent.sid = sid;
                    rVOAgent.type = 1;
                    leftSoliderAgent.Add(sid, rVOAgent);
                }
            }
        }

        // right solider
        for(int i = 0; i < 10; i++){
            for(int j = 0; j < 10; j++){
                float x = 90 - i * 3;
                float z = 15 - j * 3;
                int sid = Simulator.Instance.addAgent(new RVO.Vector2(x, z));
                if(sid >= 0){
                    GameObject tmp = GameObject.Instantiate(bluePrefab, new Vector3(x, 1f, z), Quaternion.identity);
                    tmp.name = "agent" + sid;
                    RVOAgent rVOAgent = tmp.GetComponent<RVOAgent>();
                    rVOAgent.sid = sid;
                    rVOAgent.type = 2;
                    rightSoliderAgent.Add(sid, rVOAgent);
                }
            }
        }
    }
}
