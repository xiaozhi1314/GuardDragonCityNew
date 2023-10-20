using System;
using System.Collections;
using System.Collections.Generic;
using BigDream;
using dyCloudUnitySDK;
using UnityEngine;

public class DouYinManager : MonoSingleton<DouYinManager>
{
    public override void Init(){
        try{
            string[] args = Environment.GetCommandLineArgs();
            var token = args[1];
            DYCloud cloud = new DYCloud(token, "APPID", "ENVID", "SERVICEID");

        }catch(Exception e){
            Debug.Log(e);
        }
    }
}