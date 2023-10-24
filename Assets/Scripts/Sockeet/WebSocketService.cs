using System;
using UnityEngine;
using BestHTTP.WebSocket;
using BigDream;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Object = System.Object;

public class WebSocketService : MonoSingleton<WebSocketService>
{
    // Start is called before the first frame update
    protected WebSocket wsClient = null;
    public override void Init()
    {
         wsClient = new WebSocket(new Uri("ws://176.122.178.158:8080"));
         if( wsClient != null){
            wsClient.OnOpen     += OnWebSocketOpen;
            wsClient.OnMessage  += OnWebSocketMessage;
            wsClient.OnClosed   += OnWebSocketClosed;
            wsClient.OnError    += OnWebSocketError;
         }
         wsClient.Open();
    }

    private void OnWebSocketOpen(WebSocket ws)
    {
        Common.Slog("OnWebSocketOpen");

        var tiktokToken = DouYinManager.Instance.GetToken();
        if (!DouYinManager.Instance.IsInitSuccess())
        {
            Common.SError("抖音初始化失败");
            return;
        }

        if (string.IsNullOrEmpty(tiktokToken))
        {
            Common.SError("抖音没用初始化");
            return;
        }
        // 发送登陆协议
        OnSendMessage(GameMsg.cmdType.login, new GameMsg.LoginReq()
        {
            deviceId = GameManager.Instance.GetDevicesId(),
            tiktokToken =  tiktokToken
        });
    }
    
    private void OnWebSocketMessage(WebSocket ws, string msg)
    {
        if(!string.IsNullOrEmpty(msg))
        {
            var msgObject = JObject.Parse(msg);
            if (msgObject.ContainsKey("code") )
            {
                if ((int)msgObject["code"] == 100)
                {
                    SocketMessageCallBack((GameMsg.cmdType)(int)msgObject["cmdType"], msgObject["data"]);
                }
                else
                {
                    Common.Slog($"Socket Error  code {(int)msgObject["code"]}");
                }
            }
            else
            {
                Common.Slog("Socket Error 无 code");
            }
        }
       
    }

    private void OnWebSocketClosed(WebSocket ws, ushort code, string message)
    {
        Common.Slog("OnWebSocketClosed");
    }

    private void OnWebSocketError(WebSocket ws, string error)
    {
        Common.Slog("OnWebSocketError");
        // Debug.Log(error);
    }


    /// <summary>
    /// 协议发送
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    public void OnSendMessage<T>(GameMsg.cmdType cmdType, T obj)
    {
        if(wsClient != null)
        {
            var msgObject = new JObject()
            {
                {"cmdType", (int)cmdType },
                {"data", JObject.FromObject(obj)},
            };
            wsClient.Send(msgObject.ToString());
        }
    }


    public void SocketMessageCallBack(GameMsg.cmdType cmdType, JToken jToken)
    {
        switch (cmdType)
        {
            case GameMsg.cmdType.login:
            {
                var loginRespData = jToken.ToObject<GameMsg.LoginRespData>();
                GameManager.Instance.m_SessionId = loginRespData.sessionId;
                break;
            } 
            case GameMsg.cmdType.StartGame:
            {
               
                break;
            } 
            case GameMsg.cmdType.NoticeMsg:
            {
                var moticeMsgRespDataList = jToken.ToObject<List<GameMsg.NoticeMsgRespData>>();
                EventManager.Instance.Fire(Common.EventCmd.NoticeMsg, new EventParams(Common.EventCmd.NoticeMsg,new Dictionary<string, object>()
                {
                    {"Data" , moticeMsgRespDataList},
                }));
                break;
            } 
            case GameMsg.cmdType.Result:
            {
                var resultRespIteme = jToken.ToObject<List<GameMsg.ResultRespIteme>>();
                EventManager.Instance.Fire(Common.EventCmd.RankResult, new EventParams(Common.EventCmd.NoticeMsg,new Dictionary<string, object>()
                {
                    {"Data" , resultRespIteme},
                }));
                
                break;
            } 
            default:
                Common.Slog("不存在的协议");
                break;

        }
    }
}