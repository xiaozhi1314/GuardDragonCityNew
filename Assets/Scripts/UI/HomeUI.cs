using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using BigDream;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    /// <summary>
    /// 我方hp
    /// </summary>
    public Slider redHpSlider;

    public TextMeshProUGUI redHpText;
    
    /// <summary>
    /// 敌方血量
    /// </summary>
    public Slider buleHpSlider;
    public TextMeshProUGUI buleHpText;


    /// <summary>
    /// 克隆我方信息
    /// </summary>
    public PlayerInfo cloneOwnPlayerInfo;
     
    /// <summary>
    /// 克隆敌方信息
    /// </summary>
    public PlayerInfo cloneEnemyPlayerInfo;


    private Dictionary<Common.CampType, List<PlayerInfo>> playerInfoDic;
    
    /// <summary>
    /// 当前显示最小分数
    /// </summary>
    private Dictionary<Common.CampType, int> m_MinSource;


    public void Start()
    {
        EventManager.Instance.Subscribe(Common.EventCmd.SubBuildHp, this, SubBuildHpCallBack);
        EventManager.Instance.Subscribe(Common.EventCmd.RankUpdate, this, RankUpdateCallBack);
        EventManager.Instance.Subscribe(Common.EventCmd.ResetGame, this, ResetGameCallBack);
        ResetData();
    }

    public void ResetData()
    {
        // 当前初始化主建筑的血量
        var redData =  TableManager.Instance.GetArrayData<TableMasterData>(1);
        var blueData =  TableManager.Instance.GetArrayData<TableMasterData>(1);
        EventManager.Instance.Fire(Common.EventCmd.SubBuildHp, new EventParams(Common.EventCmd.SubBuildHp, new Dictionary<string, object>()
        {
            {"CampType", Common.CampType.Red},
            {"Hp", redData.HP},
            {"MaxHp", redData.MaxHP},
            {"isAction", false},
        }));
       
        EventManager.Instance.Fire(Common.EventCmd.SubBuildHp, new EventParams(Common.EventCmd.SubBuildHp, new Dictionary<string, object>()
        {
            {"CampType", Common.CampType.Bule},
            {"Hp", blueData.HP},
            {"MaxHp", blueData.MaxHP},
            {"isAction", false},
        }));

        m_MinSource = new Dictionary<Common.CampType, int>();
        m_MinSource.Add(Common.CampType.Bule,99999999);
        m_MinSource.Add(Common.CampType.Red,99999999);
    }

    /// <summary>
    /// 血量减少
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="userData"></param>
    /// <param name="e"></param>
    private void SubBuildHpCallBack(object sender = null, object userData = null, EventParams e = null)
    {
        if (e != null && e.Objects.ContainsKey("CampType"))
        {
            var campType = (Common.CampType)e.Objects["CampType"];
            var curHp = (float)e.Objects["Hp"];
            var maxHp = (float)e.Objects["MaxHp"];
            var iaAction = (bool)e.Objects["isAction"];
            if (campType == Common.CampType.Red)
            {
                UpdateHpInfo(redHpSlider, redHpText, Convert.ToInt32(curHp), Convert.ToInt32(maxHp), iaAction);
            }
            else
            {
                UpdateHpInfo(buleHpSlider, buleHpText, Convert.ToInt32(curHp),Convert.ToInt32( maxHp), iaAction);
            }
        }
    }

    private void RankUpdateCallBack(object sender = null, object userData = null, EventParams e = null)
    {
        if (e != null && e.Objects.ContainsKey("camp"))
        {
            var camp = (Common.CampType)e.Objects["camp"];
            var tikTokId = (string)e.Objects["tikTokId"];
            var source = (int)e.Objects["source"];
            if (source < m_MinSource[camp] && m_MinSource[camp] != 99999999) return; // 去除根本不会上榜的消息
            var rankList = GameManager.Instance.GetRankList(camp);
            for (int idx = 0; idx < rankList.Count; idx++)
            {
                SetPlayerInfo(new PlayerInfoData(idx, rankList[idx].openId, rankList[idx].score), camp);
                m_MinSource[camp] = Mathf.Min(m_MinSource[camp], rankList[idx].score);
            }
            
           
        }
    }


    /// <summary>
    /// 设置阵营的信息
    /// </summary>
    /// <param name="playerInfoData"></param>
    /// <param name="campe"></param>
    public void SetPlayerInfo(PlayerInfoData playerInfoData, Common.CampType camp)
    {   
        if(playerInfoDic.ContainsKey(camp) == false)
        {
            playerInfoDic.Add(camp, new List<PlayerInfo>());
        }

        if(playerInfoDic[camp].Count <= playerInfoData.Index)
        {
            if(camp == Common.CampType.Red)
            {
                playerInfoDic[camp].Add(GameObject.Instantiate(cloneOwnPlayerInfo.gameObject, cloneOwnPlayerInfo.transform.parent, false).GetComponent<PlayerInfo>());
            }
            else if(camp == Common.CampType.Bule)
            {
                playerInfoDic[camp].Add(GameObject.Instantiate(cloneEnemyPlayerInfo.gameObject, cloneEnemyPlayerInfo.transform.parent, false).GetComponent<PlayerInfo>());
            }
         
        }    

            playerInfoDic[camp][playerInfoData.Index].SetPlayerInfo(playerInfoData);

    }

    public void UpdateHpInfo(Slider slider, TextMeshProUGUI textMeshProUGUI, int hp, int maxHp, bool isAction)
    {
        if (isAction)
        {
            slider.value = hp * 1.0f / maxHp;
            textMeshProUGUI.text = $"{hp} / {maxHp}";
        }
        else
        {
            slider.value = hp * 1.0f / maxHp;
            textMeshProUGUI.text = $"{hp} / {maxHp}";
        }
    }


    public void ResetGameCallBack(object sender = null, object userData = null, EventParams e = null)
    {
        ResetData();
    }

}
