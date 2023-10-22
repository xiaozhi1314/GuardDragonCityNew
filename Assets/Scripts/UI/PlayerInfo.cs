using System.Collections;
using System.Collections.Generic;
using BigDream;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    /// <summary>
    /// 头像
    /// </summary>
    public Image headImage;
    
    /// <summary>
    /// 排名 
    /// </summary>
    public TextMeshProUGUI randText;
    
    /// <summary>
    /// 名字
    /// </summary>
    public Text nameText;

    public PlayerInfoData playerInfoData;

    public void SetPlayerInfo(PlayerInfoData infoData)
    {
        gameObject.SetActive(true);
        if (playerInfoData == null || playerInfoData.TikTokId != infoData.TikTokId)
        {
            playerInfoData = infoData;
            if (GameManager.Instance.m_NoticeMsgDic.ContainsKey(infoData.TikTokId))
            {
                var tiktokInfo = GameManager.Instance.m_NoticeMsgDic[infoData.TikTokId];
                GameManager.Instance.LoadWebTexture(headImage,tiktokInfo.avatarUrl);
                nameText.text = tiktokInfo.nickName;
            }
        }
        randText.text = (playerInfoData.Index + 1).ToString();
        
    }

}
