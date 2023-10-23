using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
            EventManager.Instance.Subscribe(Common.EventCmd.ResetGame, this, ResetGameCallBack);
            m_ScoreDic = new Dictionary<Common.CampType, Dictionary<string, GameMsg.ResultSourceData>>();
            m_NoticeMsgDic = new Dictionary<string, GameMsg.NoticeMsgRespData>();
            // 初始化对象池  
            TableManager.Instance.GetArrayDatasGroup<TableMasterData>().Values.ToList().ForEach(masterDataObject =>
            {
                var masterData = (TableMasterData)masterDataObject;
                if (masterData.ID >= 3)
                {
                    PoolManager.Instance.SetPoolGameObject(masterData.PoolName, Resources.Load<GameObject>(masterData.PrefabPath));
                    PoolManager.Instance.SetPoolGameObject(masterData.PoolName + "Notice", Resources.Load<GameObject>(masterData.NoticePrefabPath));

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
                
                WebSocketService.Instance.OnSendMessage(GameMsg.cmdType.GM, new GameMsg.GmRep(){gmType = 1});
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
            
            if (m_NoticeMsgDic.ContainsKey(openId))
            {
                var camp = (Common.CampType)m_NoticeMsgDic[openId].camp;
                if (m_ScoreDic[camp].ContainsKey(openId) == false)
                {
                    m_ScoreDic[camp].Add(openId, new GameMsg.ResultSourceData(){openId = openId,score = 0});
                }

                m_ScoreDic[camp][openId].score += source;
                
                EventManager.Instance.Fire(Common.EventCmd.RankUpdate, new EventParams(Common.EventCmd.RankUpdate, new Dictionary<string, object>()
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
            if (m_NoticeMsgDic.ContainsKey(noticeMsgRespData.openId) == false)
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

        /// <summary>
        /// 加载网络图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="url"></param>
        public void LoadWebTexture(Image loadImg, string url)
        {
            StartCoroutine(WWWGetData(loadImg, url));

        }
        IEnumerator  WWWGetData(Image loadImg, string url)
        {
            WWW www = new WWW(url);//用WWW加载网络图片
            yield return www;
            //Myimage = transform.GetComponent<Image>();
            if (www != null && string.IsNullOrEmpty(www.error))
            {
                //获取Texture
                Texture2D texture = www.texture;
                //因为我们定义的是Image，所以这里需要把Texture2D转化为Sprite
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                loadImg.sprite = sprite;
                loadImg.SetNativeSize();
            }
        }

        /// <summary>
        /// 游戏进行结算
        /// </summary>
        /// <param name="campType">胜利方阵营</param>
        public void SetGameResult(Common.CampType campType)
        {
            var lists = new List<GameMsg.ResultSourceData>();
            m_ScoreDic.Values.ToList().ForEach(resultSourceDataDic => { lists.AddRange(resultSourceDataDic.Values.ToList());});
            lists = lists.OrderByDescending(data => data.score).ToList(); // 从大到小排序
            GameState = Common.GameState.Result;
            var resultRep = new GameMsg.ResultRep();
            resultRep.victory = (int)campType;
            resultRep.sourceData = lists;
            WebSocketService.Instance.OnSendMessage(GameMsg.cmdType.Result, resultRep);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="userData"></param>
        /// <param name="e"></param>
        public void ResetGameCallBack(object sender = null, object userData = null, EventParams e = null)
        {
            GameReset();
            MessageManager.Instance.Reset();
            RVOManager.Instance.Reset();
        }

    }
}