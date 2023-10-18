using System.Collections;
using System.Collections.Generic;
using BigDream;
using UnityEngine;

public class Root : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TableManager.Instance.Init();
        MessageManager.Instance.Init();
        EventManager.Instance.Init();
        RVOManager.Instance.Init();
        WebSocketService.Instance.Init();
    }

  
}
