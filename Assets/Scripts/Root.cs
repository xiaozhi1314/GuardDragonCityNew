using System.Collections;
using System.Collections.Generic;
using BigDream;
using UnityEngine;

public class Root : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        PoolManager.Instance.Init();
        EventManager.Instance.Init();
        TableManager.Instance.Init();
        MessageManager.Instance.Init();
        RVOManager.Instance.Init();
        WebSocketService.Instance.Init();
        InputManager.Instance.Init();
        GameManager.Instance.Init();
    }

  
}
