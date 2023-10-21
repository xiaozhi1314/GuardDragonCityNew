using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using DG.Tweening;
using UnityEngine;

namespace BigDream
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public Common.GameState GameState = Common.GameState.Start;
        public string m_SessionId = string.Empty;
        public Dictionary<string, int> m_ScoreDic;
        public Dictionary<string, GameMsg.NoticeMsgRespData> m_NoticeMsgDic;


        public override void Init()
        {
            m_ScoreDic = new Dictionary<string, int>();
            m_NoticeMsgDic = new Dictionary<string, GameMsg.NoticeMsgRespData>();
            // 初始化对象池  
            TableManager.Instance.GetArrayDatasGroup<TableMasterData>().Values.ToList().ForEach(masterDataObject =>
            {
                var masterData = (TableMasterData)masterDataObject;
                if (masterData.ID >= 3)
                {
                    PoolManager.Instance.SetPoolGameObject(masterData.PoolName, Resources.Load<GameObject>(masterData.PrefabPath));
                }
            });
            
            // 游戏开始
            GameStart();
        }

        public void GameReset()
        {
            m_ScoreDic.Clear();
            m_NoticeMsgDic.Clear();
            m_SessionId = string.Empty;
        }

        /// <summary>
        /// 游戏开始
        /// </summary>

        public void GameStart()
        {
            GameReset();
            DOTween.Sequence().AppendInterval(3.0f).AppendCallback(() =>
            {
                GameManager.Instance.GameState = Common.GameState.Playing;
                WebSocketService.Instance.OnSendMessage(GameMsg.cmdType.StartGame, new GameMsg.StartGameReq()
                {
                    sessionId = m_SessionId
                });
            });
            
            // 添加小兵
            RVOManager.Instance.AutoCreateSolider();
            
            // 添加大兵
            RVOManager.Instance.AutoCreateBigSolider();
        }


        /// <summary>
        /// 添加积分
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="source"></param>
        public void AddScore(string openId, int source)
        {
            if (openId == string.Empty) return;
            if (m_ScoreDic.ContainsKey(openId) == false)
            {
                m_ScoreDic.Add(openId, 0);
            }

            m_ScoreDic[openId] += source;
        }

        /// <summary>
        /// 添加抖音个人
        /// </summary>
        /// <param name="noticeMsgRespData"></param>
        public void AddNoticeMsg(GameMsg.NoticeMsgRespData noticeMsgRespData)
        {
            if (m_NoticeMsgDic.ContainsKey(noticeMsgRespData.openId))
            {
                m_NoticeMsgDic.Add(noticeMsgRespData.openId, noticeMsgRespData);
            }
        }


        /// <summary>
        /// 获取用户的devicesID
        /// </summary>
        /// <returns></returns>
        public string GetDevicesId()
        {
            if (PlayerPrefs.HasKey("DevicesId") == false)
            {
                PlayerPrefs.SetString("DevicesId", SystemInfo.deviceUniqueIdentifier + UnityEngine.Random.Range(0, 1000000) );
                PlayerPrefs.Save();
            }

            return PlayerPrefs.GetString("DevicesId");
        }

    }
}