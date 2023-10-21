using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
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
    

    public void Start()
    {
        /*
        playerInfoDic = new Dictionary<Common.CampeType, List<PlayerInfo>>();

        PlayerInfoData playerInfoData1 = new PlayerInfoData(0, "111", 100, "nihao");
        PlayerInfoData playerInfoData2 = new PlayerInfoData(1, "222", 50, "nihao");
        PlayerInfoData playerInfoData3 = new PlayerInfoData(2, "333", 30, "nihao");
        SetPlayerInfo(playerInfoData1, Common.CampeType.Own);
        SetPlayerInfo(playerInfoData2, Common.CampeType.Own);
        SetPlayerInfo(playerInfoData3, Common.CampeType.Own);
        SetPlayerInfo(playerInfoData1, Common.CampeType.Enemy);
        SetPlayerInfo(playerInfoData2, Common.CampeType.Enemy);
        SetPlayerInfo(playerInfoData3, Common.CampeType.Enemy);*/

        EventManager.Instance.Subscribe(Common.EventCmd.SubBuildHp, this, SubBuildHpCallBack);
        EventManager.Instance.Subscribe(Common.EventCmd.RankUpdate, this, RankUpdateCallBack);
        
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
        if (e != null && e.Objects.ContainsKey("Data"))
        {
            var masterData = JsonConvert.DeserializeObject<Common.MasterData>((string)e.Objects["Data"]);

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

}
