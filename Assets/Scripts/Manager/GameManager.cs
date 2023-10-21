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
        public Dictionary<Common.CampType,Dictionary<string, GameMsg.ResultSourceData> > m_ScoreDic;
        public Dictionary<string, GameMsg.NoticeMsgRespData> m_NoticeMsgDic;


        public override void Init()
        {
            m_ScoreDic = new Dictionary<Common.CampType, Dictionary<string, GameMsg.ResultSourceData>>();
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
            m_ScoreDic.Add(Common.CampType.Bule, new Dictionary<string, GameMsg.ResultSourceData>());
            m_ScoreDic.Add(Common.CampType.Red, new Dictionary<string, GameMsg.ResultSourceData>());
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
            
            if (m_NoticeMsgDic.ContainsKey(openId) == false)
            {
                var camp = (Common.CampType)m_NoticeMsgDic[openId].camp;
                if (m_ScoreDic[camp].ContainsKey(openId) == false)
                {
                    m_ScoreDic[camp].Add(openId, new GameMsg.ResultSourceData(){openId = openId,score = 0});
                }

                m_ScoreDic[camp][openId].score += source;
                
                EventManager.Instance.Fire(Common.EventCmd.RankUpdate, new EventParams(Common.EventCmd.NoticeMsg, new Dictionary<string, object>()
                {
                    {"camp", m_NoticeMsgDic[openId].camp},
                    {"tikTokId", openId},
                    {"source", m_ScoreDic[camp][openId].score}
                }));
            }
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

        /// <summary>
        /// 根据阵营去出前三名
        /// </summary>
        /// <param name="campType"></param>
        /// <returns></returns>
        public List<GameMsg.ResultSourceData> GetRankList(Common.CampType campType)
        {
            return m_ScoreDic[campType].Values.ToList().OrderByDescending(data => data.score).Take(3).ToList();
        }

    }
}