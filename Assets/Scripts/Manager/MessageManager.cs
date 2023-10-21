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

            /**
           var temp  = TableManager.Instance.GetArrayData<TableMasterData>(1);
           var i = 1;

            EventManager.Instance.Subscribe(Common.EventCmd.AddMaster, this, AddMasterCallBack);
            **/
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
                var masterData = JsonConvert.DeserializeObject<Common.MasterData>((string)e.Objects["Data"]);
                
            }
        }


        /// <summary>
        /// 消息添加怪物

        public void AddMaster(Common.MasterData masterData)
        {
            var objs = new Dictionary<string, object>() { { "Data", JsonConvert.SerializeObject(masterData) } };

            EventManager.Instance.Fire(Common.EventCmd.AddMaster, new EventParams(Common.EventCmd.AddMaster, objs));
        }

    }
}