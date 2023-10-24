using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BigDream;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{

    /// <summary>
    /// 触摸屏蔽层
    /// </summary>
    public Image m_MashImageLayer;

    public Image m_BgImage;

    public List<RankItemInfo> m_RankItemInfo;

    public RankItemInfo m_CloneRankItemInfo;

    private List<RankItemInfo> m_LoadRankItemInfo = new List<RankItemInfo>();

    /// <summary>
    /// 展示ui
    /// </summary>
    public void Open(List<GameMsg.ResultSourceData> sourceData)
    {

        if (m_LoadRankItemInfo.Count == 0)
        {
            m_LoadRankItemInfo.AddRange(m_RankItemInfo);
        }

        var delayTime = 0.5f;
        gameObject.SetActive(true);
        m_MashImageLayer.gameObject.SetActive(true);
        m_BgImage.transform.localScale = Vector3.one * 0.00001f;
        DOTween.Sequence().Append(m_BgImage.transform.DOScale(UnityEngine.Vector3.one, delayTime).SetEase(Ease.OutBack).OnComplete(() =>
        {
            m_MashImageLayer.gameObject.SetActive(false);
            OpenCover();
        }));
        

        var count = m_LoadRankItemInfo.Count;
        for (int idx = count; idx < sourceData.Count; idx++)
        {
            var cloneGameObject = Instantiate(m_CloneRankItemInfo.gameObject, m_CloneRankItemInfo.transform.parent, false);
            cloneGameObject.gameObject.SetActive(true);
            m_LoadRankItemInfo.Add(cloneGameObject.GetComponent<RankItemInfo>());
        }

        for (int idx = sourceData.Count; idx < m_LoadRankItemInfo.Count; idx++)
        {
            m_LoadRankItemInfo[idx].gameObject.SetActive(true);
        }
        

        // 4到无限
        for (int idx = 0; idx < sourceData.Count; idx++)
        {
            
            var tikTokInfo = GameManager.Instance.m_NoticeMsgDic[sourceData[idx].openId];
            m_LoadRankItemInfo[idx].UpdateInfo(idx, tikTokInfo.avatarUrl, tikTokInfo.nickName, sourceData[idx].score, 0, 0);
        }
        
    }

    // 关闭ui
    public void Close()
    {
        var delayTime = 0.5f;
        m_MashImageLayer.gameObject.SetActive(true);
        DOTween.Sequence().Append(m_BgImage.transform.DOScale(UnityEngine.Vector3.zero, delayTime).SetEase(Ease.OutBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
            CloseCover();
        }));
    }


    
    /// <summary>
    /// 打开结束的ui回调
    /// </summary>
    private void OpenCover()
    {
        
    }
    
    /// <summary>
    /// 关闭结束回调
    /// </summary>
    private void CloseCover()
    {
        // 重启游戏
        EventManager.Instance.Fire(Common.EventCmd.ResetGame, new EventParams(Common.EventCmd.ResetGame, new Dictionary<string, object>()
        {
          
        }));

    }

    public void OnCloseButton()
    {
        Close();
    }
}
