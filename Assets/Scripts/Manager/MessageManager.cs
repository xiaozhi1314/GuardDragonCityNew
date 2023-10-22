using System;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace BigDream
{
    public class MessageManager : MonoSingleton<MessageManager>
    {
        // 当前显示的动画队列
        private List<GameMsg.NoticeMsgRespData> m_ShowNoticeAction;
        // 当前是否正在显示动画
        private bool m_isShowActioning = false;

        /// <summary>
        /// 消息的父节点
        /// </summary>
        private GameObject m_messageParent = null;
        
        public override void Init()
        {
            // 服务器消息通知
            EventManager.Instance.Subscribe(Common.EventCmd.NoticeMsg, this, AddMasterCallBack);
            m_ShowNoticeAction = new List<GameMsg.NoticeMsgRespData>();
            m_messageParent = GameObject.Find("MessageUI");

        }

        public void Reset()
        {
            m_ShowNoticeAction.Clear();
        }

        
        /// <summary>
        /// 添加兵种的回调 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="userData"></param>
        /// <param name="e"></param>
        private void AddMasterCallBack(object sender = null, object userData = null, EventParams e = null)
        {
            if (e != null && e.Objects.ContainsKey("Data"))
            {
                var noticeMsgRespData = (List<GameMsg.NoticeMsgRespData>)e.Objects["Data"];
                noticeMsgRespData.ForEach(data =>
                {
                    GameManager.Instance.AddNoticeMsg(data);
                    AddGiftNotice(data);
                   
                });

            }
        }
        
        /// <summary>
        /// 添加当前动画信息
        /// </summary>
        /// <param name="noticeMsgRespData"></param>
        public void AddGiftNotice(GameMsg.NoticeMsgRespData noticeMsgRespData)
        {
            if (noticeMsgRespData.soliderId >= 5) // 小兵没有动画
            {
                m_ShowNoticeAction.Add(noticeMsgRespData);
                RunNoticeUiAction();
            }
        }
    
        // 执行当前动画
        public void RunNoticeUiAction()
        {
            if (m_isShowActioning) return;
            m_isShowActioning = true;
            ShowNextAction();
        }

        // 设置下一个action
        public void ShowNextAction()
        {
            if (m_ShowNoticeAction.Count == 0)
            {
                m_isShowActioning = false;
                return;
            }
            // 获取数据中的第一个
            var curMessageData = m_ShowNoticeAction[0];
            m_ShowNoticeAction.RemoveAt(0);
            // 创建小兵
            RVOManager.Instance.CreateSolider(curMessageData.soliderId, curMessageData.soliderCount, (Common.CampType)curMessageData.camp, curMessageData.openId);
            // 播放UI动画
            var masterData = TableManager.Instance.GetArrayData<TableMasterData>(curMessageData.soliderId);
            if (masterData != null)
            {
                
                var showAction  = PoolManager.Instance.GetObj(masterData.PoolName + "Notice", m_messageParent.transform,UnityEngine.Vector3.zero);
                showAction.transform.localPosition = Vector3.zero;
                showAction.transform.localScale =  UnityEngine.Vector3.zero;
                showAction.SetActive(true);
                // 从小到大时间
                var scaleTime = 0.3f;
                // 停留时间
                var showTime = 3.0f;

                DOTween.Sequence().Append(showAction.transform.DOScale(Vector3.one, scaleTime)).AppendInterval(showTime).AppendCallback(() =>
                {
                    PoolManager.Instance.FreeObj(masterData.PoolName + "Notice", showAction);
                    ShowNextAction();
                });   
                
            }

            
            
        }




    }
}