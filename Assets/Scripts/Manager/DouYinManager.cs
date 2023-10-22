using System;
using System.Collections;
using System.Collections.Generic;
using BigDream;
using dyCloudUnitySDK;
using UnityEngine;

public class DouYinManager : MonoSingleton<DouYinManager>
{
    
    private string m_token = String.Empty;
    private bool m_IsInitSuccess = false;
    public override void Init(){
        try{
            string[] args = Environment.GetCommandLineArgs();
            m_token = args[1];
            DYCloud cloud = new DYCloud(m_token, "APPID", "ENVID", "SERVICEID");
            m_IsInitSuccess = true;

        }catch(Exception e){
            Debug.Log(e);
        }
    }

    public string GetToken()
    {
        return m_token;
    }

    public bool IsInitSuccess()
    {
        return m_IsInitSuccess;
    }
}