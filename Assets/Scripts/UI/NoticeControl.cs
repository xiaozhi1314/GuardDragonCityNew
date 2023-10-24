using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoticeControl : MonoBehaviour
{
    /// <summary>
    /// 显示ui
    /// </summary>
    private List<NoticeUI> m_ShowNoticeUI;

    /// <summary>
    /// 当前空闲槽位占用
    /// </summary>
    private List<bool> m_FreeActionShow;

    private List<GameMsg.ResultSourceData> m_ShowResultSourceData;

    public void Awake()
    {
        m_ShowNoticeUI = transform.GetComponentsInChildren<NoticeUI>(true).ToList();
        Reset();
    }

    public void Reset()
    {
        m_FreeActionShow = Enumerable.Repeat(true, m_ShowNoticeUI.Count).ToList();
        m_ShowResultSourceData = new List<GameMsg.ResultSourceData>();
    }

    public void AddNotice(GameMsg.ResultSourceData resultSourceData)
    {
        m_ShowResultSourceData.Add(resultSourceData);
        ShowNextAction();
    }

    public void ShowNextAction()
    {
        if(m_ShowResultSourceData.Count == 0) return;
        for (int idx = 0; idx < m_FreeActionShow.Count; idx++)
        {
            if (m_FreeActionShow[idx])
            {
                m_FreeActionShow[idx] = false;
                m_ShowNoticeUI[idx].UpdateData(m_ShowResultSourceData[0], () =>
                {
                    m_FreeActionShow[idx] = true;
                    ShowNextAction();
                });
                m_ShowResultSourceData.RemoveAt(0);
                return;
            }
        }
    }
}
