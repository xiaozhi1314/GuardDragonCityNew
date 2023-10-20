using System;
using UnityEngine;
using BestHTTP.WebSocket;
using BigDream;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Newtonsoft.Json;

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

    private void OnWebSocketOpen(WebSocket ws){
        // Debug.Log("OnWebSocketOpen");
    }
    
    private void OnWebSocketMessage(WebSocket ws, string msg){
        if(msg != null){
            GameMsg gameMsg = JsonUtility.FromJson<GameMsg>(msg);
            if(gameMsg == null || gameMsg.code == 0){
                return;
            }
            // RVOManager.Instance.CreateBigSolider(gameMsg.data[0].type, gameMsg.data[0].subtype);

            var masterData = new Common.MasterData();
            masterData.ID = gameMsg.subtype;
            masterData.CampType = (gameMsg.type == 1 ? Common.CampType.Red : Common.CampType.Bule);
            masterData.Count = 2;
            var objs = new Dictionary<string, object>() { { "Data", JsonConvert.SerializeObject(masterData) } };
            EventManager.Instance.Fire(Common.EventCmd.AddSolider, new EventParams(Common.EventCmd.AddSolider, objs));
            // switch(gameMsg.data[0].type){
            //     case 0:
            //         RVOManager.Instance.CreateBigSolider(gameMsg.data[0].type, gameMsg.data[0].subtype);
            //     break;
            //     case 1:
            //         RVOManager.Instance.CreateBigSolider(gameMsg.data[0].type, gameMsg.data[0].subtype);
            //     break;
            //     // case 1 :
            //     //     // CreateBigSolider(gameMsg.subtype);   
            //     // back;
        
            // }
        }
        // Debug.Log("OnWebSocketMessage");
        // Debug.Log(msg);
    }

    private void OnWebSocketClosed(WebSocket ws, ushort code, string message){
        // Debug.Log("OnWebSocketClosed");
    }

    private void OnWebSocketError(WebSocket ws, string error){
        // Debug.Log("OnWebSocketError");
        // Debug.Log(error);
    }


    public void OnSendMessage(string msg){
        if(wsClient != null){
            wsClient.Send(msg);
        }
    }
}