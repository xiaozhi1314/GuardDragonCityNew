using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace BigDream
{
    public class InputManager : MonoSingleton<InputManager>
    {
        public override void Init()
        {

            // 键盘输入都写到这个里面
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.F1))
            {
                Debug.Log("f1");
            }
            else if (Input.GetKeyUp(KeyCode.A)) 
            {
                Debug.Log("A");
            }
            else if(Input.GetKeyUp(KeyCode.Q))
            {
                if (GameManager.Instance.m_ScoreDic[Common.CampType.Red].Count > 0)
                {
                    var firstTikTokInfo = GameManager.Instance.m_ScoreDic[Common.CampType.Red].Values.ToList().First();
                    EventManager.Instance.Fire(Common.EventCmd.RankUpdate, new EventParams(Common.EventCmd.RankUpdate, new Dictionary<string, object>()
                    {
                        {"camp", Common.CampType.Red},
                        {"tikTokId", firstTikTokInfo.openId},
                        {"source", 1000}
                    }));
                }
            }
            
            else if(Input.GetKeyUp(KeyCode.W))
            {
                if (GameManager.Instance.m_ScoreDic[Common.CampType.Bule].Count > 0)
                {
                    var firstTikTokInfo = GameManager.Instance.m_ScoreDic[Common.CampType.Bule].Values.ToList().First();
                    EventManager.Instance.Fire(Common.EventCmd.RankUpdate, new EventParams(Common.EventCmd.RankUpdate, new Dictionary<string, object>()
                    {
                        {"camp", Common.CampType.Bule},
                        {"tikTokId", firstTikTokInfo.openId},
                        {"source", 10000}
                    }));
                }
            }

        }
    }
}