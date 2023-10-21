using System;
using System.Collections.Generic;
using UnityEngine;

public class GameMsg
{
    /*
     * 
1000 登陆协议 客户端发起登陆
1001 礼物通知 服务端主动推送
1002 结算界面 客户端发起结算通知
1003 游戏开始 客户端发起结算通知
     */
    public  enum cmdType
    {
        login = 1000,
        NoticeMsg,
        Result,
        StartGame,
    }


    /// <summary>
    /// 登陆协议发送 
    /// </summary>
    public class LoginReq
    {
        public string deviceId;
        public string tiktokToken;
    }
    
    /// <summary>
    /// 登陆协议data返回
    /// </summary>
    public class LoginRespData
    {
        public string sessionId;
    }


    /// <summary>
    /// 刷礼物主动推送
    /// </summary>
    public class NoticeMsgRespData
    {
        public string msgId;
        public string msgType;
        public string openId;
        public string avatarUrl;
        public string nickName;
        public string camp;
        public string soliderId;
        public string soliderCount;
    }

    /// <summary>
    /// 结算界面子数据
    /// </summary>
    public class ResultSourceData
    {
        public string openId;
        public int score;
    }

    /// <summary>
    /// 结算请求
    /// </summary>
    public class ResultRep
    {
        public int victory;//1、2,
        public List<ResultSourceData> sourceData;
    }

    /// <summary>
    /// 结算数据返回
    /// </summary>
    public class ResultRespIteme
    {
        public string openId;
        public int rank;
        public int lastRank;
        public int victoryCount;
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public class StartGameReq
    {
        public string sessionId;
    }

}



