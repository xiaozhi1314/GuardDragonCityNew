using System;
using UnityEngine;

[Serializable]
public class GameMsg
{
    public int code;
    public int type;
    public int subtype;
    public string msg;

//     {
//     "code": 200,
//     "type": "1",
//     "subtype": "2",
//     "msg": "礼物生成大兵"
//      }
}


public class WebSocketProtocal{
    public string type;
    public GameMsg[] data;
}

