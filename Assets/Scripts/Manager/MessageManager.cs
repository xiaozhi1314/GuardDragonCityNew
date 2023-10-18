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