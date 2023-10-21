using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;

namespace BigDream
{
    public class MessageManager : MonoSingleton<MessageManager>
    {
        public override void Init()
        {
            // 服务器消息通知
            EventManager.Instance.Subscribe(Common.EventCmd.NoticeMsg, this, AddMasterCallBack);
        
        }


        /// <summary>
        /// 添加兵种的回调 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="userData"></param>
        /// <param name="e"></param>
        private void AddMasterCallBack(object sender = null, object userData = null, EventParams e = null)
        {
            if (e != null && e.Objects.ContainsKey("Data"))
            {
                var noticeMsgRespData = (List<GameMsg.NoticeMsgRespData>)e.Objects["Data"];
                noticeMsgRespData.ForEach(data =>
                {
                    RVOManager.Instance.CreateSolider(data.soliderId, data.soliderCount, (Common.CampType)data.camp, data.openId);
                });

            }
        }
        

    }
}