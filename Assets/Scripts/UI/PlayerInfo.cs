using System.Collections;
using System.Collections.Generic;
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
    public TextMeshProUGUI nameText;
    void Start()
    {
        
    }

    public void SetPlayerInfo(PlayerInfoData playerInfoData)
    {
        gameObject.SetActive(true);
        randText.text = playerInfoData.Index.ToString();
        nameText.text = playerInfoData.Name;
    }

}
