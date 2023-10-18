using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    /// <summary>
    /// 我方hp
    /// </summary>
    public Image ownHpSlider;
    
    /// <summary>
    /// 敌方血量
    /// </summary>
    public Slider enemyHpSlider;


    /// <summary>
    /// 克隆我方信息
    /// </summary>
    public PlayerInfo cloneOwnPlayerInfo;


    /// <summary>
    /// 克隆敌方信息
    /// </summary>
    public PlayerInfo cloneEnemyPlayerInfo;


    private Dictionary<Common.CampType, List<PlayerInfo>> playerInfoDic;


    public void Awake()
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
    
}
