using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DG.Tweening.Plugins.Options;
using RVO;
using Unity.VisualScripting;
using UnityEngine;

public class RVOAgent : MonoBehaviour
{
    public Common.AIAction targetAIAction;
    public int sid = -1;
    public int type;
    public Vector3 target = Vector3.zero;
    public int targetType = 0;  // 0 无 1 城堡 2 士兵
    public int targetSid = -1;
    public RVOAgent targetAgent;
    public Transform leftCastle, rightCastle;


    public bool isDead;
    // 定义一个变量hp，用来存储当前血量
    public float hp;
    // 定义一个变量maxHp，用来存储最大血量
    private float maxHp;
    // 定义一个变量atk，用来存储当前攻击力
    private float atk;
    private float atkCd;
    private float curCd;
    // 定义一个变量atkDis，用来存储攻击力距离
    private float atkDis;
    public int actionState;  // 0 无状态 1 走路 2 攻击 3 死亡
    

    // Start is called before the first frame update
    void Start()
    {
        initData();
    }

    void initData(){
        hp = 2;
        maxHp = 2;
        atk = 1;
        atkCd = 0.5f;
        atkDis = 5.0f;
        actionState = 0;
        isDead = false;
        targetType = 1;
        target = (type == 1 ? rightCastle.position : leftCastle.position);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // 非攻击状态移动
        moveSolider();

        // // 到达目标更新状态
        // checkSoliderAtkDis();

        // if(actionState == 2){
        //     if(curCd >= atkCd){
        //         // 攻击目标
        //         attackSolider();
        //     }else{
        //         curCd += Time.deltaTime;
        //     }
        // }

        // // 寻找目标
        // findAttackSolider();

        // // 更新自身状态
        // upDateSoliderState();

        // // 更新目标状态
        // upDateTargetSoliderState();
    }

    bool isSoliderDead(){
        if(isDead || hp <= 0 || sid < 0){
            return true;
        }
        return false;
    }

    bool checkTargetDeed(){
        if(targetType != 1){
            if(targetAgent != null){
                if(targetAgent.isDead || targetAgent.hp <= 0 || targetAgent.sid < 0){
                    return true;
                }
                return false;
            }else{
                return true;
            }
        }
        return false;
    }
    void moveSolider(){
        // 非攻击状态移动
        if(actionState != 2){
            if(!isSoliderDead()){
                if(!checkTargetDeed()){
                    RVO.Vector2 pos = Simulator.Instance.getAgentPosition(sid);
                    RVO.Vector2 vel = Simulator.Instance.getAgentVelocity(sid);
                    transform.position = new Vector3(pos.x(), transform.position.y, pos.y());
                    if(Mathf.Abs(vel.x()) > 0.01f && Mathf.Abs(vel.y()) > 0.01f){
                        transform.forward = new Vector3(vel.x(), 0, vel.y()).normalized;
                    }

                    RVO.Vector2 goalVector = (new RVO.Vector2(target.x, target.z) - pos); // (new RVO.Vector2(0, 0) - Simulator.Instance.getAgentPosition(sid));
                    if(RVOMath.absSq(goalVector) > 1.0f){
                        goalVector = RVOMath.normalize(goalVector) * 5.0f;
                    }
                    Simulator.Instance.setAgentPrefVelocity(sid, goalVector);
                    actionState = 1;
                }else{
                    updateAttackInfo();
                }
            }else{
                upDateSoliderState();
            }
        }
    }

    bool checkSoliderAtkDis(){
        if(actionState != 2){
            if(!isSoliderDead()){
                if(!checkTargetDeed()){
                    if(Vector3.Distance(target, transform.position) <= atkDis){
                        Simulator.Instance.setAgentPrefVelocity(sid, new RVO.Vector2(0.0f, 0.0f));
                        actionState = 2;
                        return true;
                    }
                }
            }
        }
        return false;
    }


    void attackSolider(){
        if(targetType == 1){
            return;
        }
        if(actionState == 2){
            if(!isSoliderDead()){
                if(!checkTargetDeed()){
                    if(targetAgent.hp <= 0){
                        Simulator.Instance.delAgent(targetSid);
                        // Simulator.Instance.Clear();
                        RVOManager rVOManager = GameObject.Find("Ground").GetComponent<RVOManager>();
                        // rVOManager.rightSoliderAgent.Remove(targetSid);
                        if((type == 1 ? rVOManager.rightSoliderAgent.Remove(targetSid) : rVOManager.leftSoliderAgent.Remove(targetSid))){
                            // Destroy(targetAgent.gameObject);
                            targetAgent.gameObject.SetActive(false);
                            targetAgent.isDead = true;
                            updateAttackInfo();
                        }
                    }else{
                        targetAgent.hp -= atk;
                    }
                }else{
                    updateAttackInfo();
                }
            }else{
                upDateSoliderState();
            }
        }
    }

    void findAttackSolider(){
        if(targetType == 1){
            if(!isSoliderDead()){
                int count = Simulator.Instance.getAgentNumAgentNeighbors(sid);
                if( count > 0){
                    bool isExists = false;
                    RVOManager rVOManager = GameObject.Find("Ground").GetComponent<RVOManager>();
                    for(int i = 0; i < count; i++){
                        int nid = Simulator.Instance.getAgentAgentNeighbor(sid, i);
                        isExists = (type == 1 ? rVOManager.rightSoliderAgent.ContainsKey(nid) : rVOManager.leftSoliderAgent.ContainsKey(nid));
                        if(isExists){
                            targetAgent = (type == 1 ? rVOManager.rightSoliderAgent[nid] : rVOManager.leftSoliderAgent[nid]);
                            target = targetAgent.gameObject.transform.position;
                            targetType = 2;
                            targetSid = nid;
                            break;
                        }
                    }
                    if(Vector3.Distance((type == 1 ? rightCastle.position : leftCastle.position), transform.position) <= 10.0f){
                        if(type == 1){
                            if (rVOManager.rightSoliderAgent.Count > 0)
                            {
                                var e = (type == 1 ? rVOManager.rightSoliderAgent : rVOManager.leftSoliderAgent).GetEnumerator();
                                e.MoveNext();
                                var a = (KeyValuePair<int, RVOAgent>)e.Current;
                                targetAgent = a.Value;
                                target = a.Value.gameObject.transform.position;
                                targetType = 2;
                            }
                            
                        }
                        else if(type == 2){
                            if (rVOManager.leftSoliderAgent.Count > 0){
                                var e = (type == 1 ? rVOManager.rightSoliderAgent : rVOManager.leftSoliderAgent).GetEnumerator();
                                e.MoveNext();
                                var a = (KeyValuePair<int, RVOAgent>)e.Current;
                                targetAgent = a.Value;
                                target = a.Value.gameObject.transform.position;
                                targetType = 2;
                            }
                        }
                    }else{
                        if(!isExists){
                            target = (type == 1 ? rightCastle.position : leftCastle.position);
                        }
                    }
                }else{
                    target = (type == 1 ? rightCastle.position : leftCastle.position);
                }
            }else{
                upDateSoliderState();
            }
        }
    }

    bool getNeighborTarget(){
        if(!isSoliderDead()){
            int count = Simulator.Instance.getAgentNumAgentNeighbors(sid);
            if( count > 0){
                bool isExists = false;
                RVOManager rVOManager = GameObject.Find("Ground").GetComponent<RVOManager>();
                for(int i = 0; i < count; i++){
                    int nid = Simulator.Instance.getAgentAgentNeighbor(sid, i);
                    isExists = (type == 1 ? rVOManager.rightSoliderAgent.ContainsKey(nid) : rVOManager.leftSoliderAgent.ContainsKey(nid));
                    if(isExists){
                        targetAgent = (type == 1 ? rVOManager.rightSoliderAgent[nid] : rVOManager.leftSoliderAgent[nid]);
                        target = targetAgent.gameObject.transform.position;
                        targetType = 2;
                        targetSid = nid;
                        return true;
                    }
                }
            }
        }else{
            upDateSoliderState();
        }
        return false;
    }

    void upDateSoliderState(){
        if(isDead || hp <= 0 || sid < 0){
            gameObject.SetActive(false);
        }
    }
    void upDateTargetSoliderState(){
        if(checkTargetDeed()){
            if(targetAgent != null && targetAgent.sid >= 0){
                RVOManager rVOManager = GameObject.Find("Ground").GetComponent<RVOManager>();
                var a = (targetAgent.type == 1 ? rVOManager.rightSoliderAgent.Remove(targetAgent.sid) : rVOManager.leftSoliderAgent.Remove(targetAgent.sid));
                targetAgent.gameObject.SetActive(false);
                targetAgent.isDead = true;
            }
        }
    }
    void updateAttackInfo(){
        // if(!getNeighborTarget()){
            resetInitTarget();
        // }
    }

    void resetInitTarget(){
        actionState = 0;
        targetType = 1;
        targetSid = -1;
        targetAgent = null;
        target = (type == 1 ? rightCastle.position : leftCastle.position);
    }
}
